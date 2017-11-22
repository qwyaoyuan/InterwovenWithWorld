using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TTaskEditor.Data;
using Point = System.Windows.Point;

namespace TTaskEditor.Pages
{
    /// <summary>
    /// Interaction logic for Tasks.xaml
    /// </summary>
    public partial class Tasks : UserControl
    {
        public Tasks()
        {
            InitializeComponent();
            this.PreviewMouseDown += EndTranstion;
            this.MouseMove += MakeTranstion;
            this.KeyDown += Tasks_KeyDown;
            this.Loaded += Tasks_Loaded;

            this.MouseDown += Tasks_MouseDown;
            this.MouseUp += Tasks_MouseUp;
            this.MouseMove += Tasks_MouseMove;
            InitialAllTasks();

           
        }

        private Point lastPoit;
        void Tasks_MouseMove(object sender, MouseEventArgs e)
        {
            Console.WriteLine(outerScrollViewer.HorizontalOffset);
            if (isDraging)
            { 
                Point nowPos = e.GetPosition(this);
                Vector offset = nowPos - lastPoit;
                if (Math.Abs(offset.X) > Math.Abs(offset.Y))
                {
                    outerScrollViewer.ScrollToHorizontalOffset(outerScrollViewer.HorizontalOffset + offset.X);
                }
                else
                {
                    outerScrollViewer.ScrollToVerticalOffset(outerScrollViewer.VerticalOffset + offset.Y);
                }
                lastPoit = nowPos;
            }

        }

        void Tasks_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Middle)
                isDraging = false;
        }

        private bool isDraging = false;
        void Tasks_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Middle)
            {
                isDraging = true;
                lastPoit = e.GetPosition(this);
            }
        }


        /// <summary>
        /// 当前控件加载完毕后,连线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Tasks_Loaded(object sender, RoutedEventArgs e)
        {
            Grapic<MetaTaskInfo> data = (Grapic<MetaTaskInfo>)TTaskEditor.Data.Tasks.Instance.GetType()
                .GetField("Data", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(TTaskEditor.Data.Tasks.Instance);

            //添加所有控件
            if (!File.Exists("ControlPos.txt")) return;
            if (data == null) return;
            List<MetaTaskInfo> allTaskInfos = data.AllNodes;
     
            //创建所有连线
            foreach (var taskInfo in allTaskInfos)
            {
                if (taskInfo.Children != null)
                {
                    foreach (var child in taskInfo.Children)
                    {
                        ApplicationState.MouseState = MouseState.MakeTranstion;
                        ApplicationState.TransationFromElement = expanders[taskInfo.ID];
                        ApplicationState.TransationToElement = expanders[child.ID];
                        EndTranstion(null, null);
                    }
                }

            }
            expanders = null;
        }


        Dictionary<int, Expander> expanders = new Dictionary<int, Expander>();
        /// <summary>
        /// 初始化所有已保存任务
        /// </summary>
        private void InitialAllTasks()
        {
            TTaskEditor.Data.Tasks.Instance.LoadTasks("MetaTasksData.txt");
            if (File.Exists("ControlPos.txt"))
            {
                string[] controlPoss = File.ReadAllLines("ControlPos.txt");
                foreach (var cp in controlPoss)
                {
                    string[] pos = cp.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    this.controlPos.Add(int.Parse(pos[0]), new Point(int.Parse(pos[1]), int.Parse(pos[2])));
                }
            }

            Grapic<MetaTaskInfo> data = (Grapic<MetaTaskInfo>)TTaskEditor.Data.Tasks.Instance.GetType()
                .GetField("Data", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(TTaskEditor.Data.Tasks.Instance);

            //添加所有控件
            if (!File.Exists("ControlPos.txt")) return;

            if (data == null) return;
            List<MetaTaskInfo> allTaskInfos = data.AllNodes;
     
            foreach (var taskInfo in allTaskInfos)
            {
                TaskItem taskItem = new TaskItem();
                var pos = controlPos[taskInfo.ID];
                Canvas.SetLeft(taskItem, pos.X);
                Canvas.SetTop(taskItem, pos.Y);
                MainCanvas.Children.Add(taskItem);
                allTasks.Add(taskItem);
                expanders.Add(taskInfo.ID,taskItem.TitleExpander);
            }
    
        }

        void Tasks_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.S)
            {
                SaveAllTask();
            }
        }

        /// <summary>
        /// 保存所有的task
        /// </summary>
        private void SaveAllTask()
        {
            var reslut = MessageBox.Show(App.Current.MainWindow, "是否保存", "提示", MessageBoxButton.YesNo);
            if (reslut == MessageBoxResult.No) return;
            List<MetaTaskInfo> taskInfos = allTasks.Select(t => t.MetaTaskInfo).ToList();
            if (taskInfos.Any(t => t == null))
            {
                MessageBox.Show("有任务格式错误,请修正后保存");
                return;
            }

            foreach (var taskInfo in taskInfos)
            {
                //加上所有孩子与父亲
                if (relationDic.AllKeys.Contains(taskInfo.ID.ToString()))
                {
                    string[] childsId = relationDic.GetValues(taskInfo.ID.ToString());
                    foreach (var child in childsId)
                    {
                        taskInfo.AddChild(taskInfos.Single(ti => ti.ID.ToString().Equals(child)));
                        taskInfos.Single(ti => ti.ID.ToString().Equals(child)).AddParent(taskInfo);
                    }
                }
            }
            string json = JsonConvert.SerializeObject(taskInfos, new JsonSerializerSettings() { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
            File.WriteAllText("MetaTasksData.txt", json);

            for (int i = 0; i < taskInfos.Count; i++)
            {
                taskInfos[i].MetaTaskNode.KillMonsterAssignCount = new Dictionary<int, int>();
                taskInfos[i].MetaTaskNode.GetGoodsAssignCount = new Dictionary<int, int>();
                taskInfos[i].MetaTaskNode.ArriveAssignPosition = new Vector3(0,0,0);
                taskInfos[i].MetaTaskNode.TimeLimit = 0;
            }

            string json2 = JsonConvert.SerializeObject(taskInfos, new JsonSerializerSettings() { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
            File.WriteAllText("TasksInitialData.txt", json2);
            ////进行序列化
            //保存所有控件位置
            using (StreamWriter sw = new StreamWriter("ControlPos.txt"))
            {
                foreach (var cp in controlPos)
                {
                    sw.WriteLine(cp.Key + "," + cp.Value.X + "," + cp.Value.Y);
                }
            }
            MessageBox.Show("保存成功");

        }



        NameValueCollection relationDic = new NameValueCollection();
        private void EndTranstion(object sender, MouseButtonEventArgs e)
        {
            if (ApplicationState.MouseState != MouseState.MakeTranstion) return;
            if (ApplicationState.TransationFromElement != null && ApplicationState.TransationToElement != null)
            {
                var desPoint = ApplicationState.TransationToElement.TransformToAncestor(MainCanvas).Transform(new Point(ApplicationState.TransationToElement.ActualWidth / 2, ApplicationState.TransationToElement.ActualHeight / 2));
                var fromPoint =
                    ApplicationState.TransationFromElement.TransformToAncestor(MainCanvas).Transform(new Point(ApplicationState.TransationFromElement.ActualWidth / 2, ApplicationState.TransationFromElement.ActualHeight / 2));
                if (lineWithArrow != null)
                    MainCanvas.Children.Remove(lineWithArrow);

                //保存关系

                string formKey = (ApplicationState.TransationFromElement as Expander).Header.ToString().Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries)[1];
                string toKey = (ApplicationState.TransationToElement as Expander).Header.ToString().Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries)[1];


                relationDic.Add(formKey, toKey);

                lineWithArrow = DrawLinkArrow(fromPoint, desPoint);
                MainCanvas.Children.Add(lineWithArrow);
                lines.Add(lineWithArrow);
                ApplicationState.MouseState = MouseState.None;
                ApplicationState.TransationFromElement = null;
                ApplicationState.TransationToElement = null;
                lineWithArrow = null;

            }
        }

        private List<Shape> lines = new List<Shape>();


        /// <summary>
        /// 当前的箭头键
        /// </summary>
        private Shape lineWithArrow;
        /// <summary>
        /// 连线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MakeTranstion(object sender, MouseEventArgs e)
        {
            if (ApplicationState.MouseState == MouseState.MakeTranstion)
            {
                if (ApplicationState.TransationFromElement != null && ApplicationState.TransationToElement == null)
                {

                    var desPoint = Mouse.GetPosition(MainCanvas);
                    var fromPoint =
                        ApplicationState.TransationFromElement.TransformToAncestor(MainCanvas).Transform(new Point(ApplicationState.TransationFromElement.ActualWidth / 2, ApplicationState.TransationFromElement.ActualHeight / 2));
                    if (lineWithArrow != null)
                        MainCanvas.Children.Remove(lineWithArrow);
                    lineWithArrow = DrawLinkArrow(fromPoint, desPoint);
                    lineWithArrow.IsHitTestVisible = false;
                    MainCanvas.Children.Add(lineWithArrow);
                }
            }
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="startPoint">线条开始点</param>
        /// <param name="endPoint">线条结束点</param>
        /// <returns></returns>
        private Point[] GetCapTraingle(Point startPoint, Point endPoint, float triangleHeight)
        {
            double k = ((double)(endPoint.Y - startPoint.Y)) / (endPoint.X - startPoint.X);
            double b = startPoint.Y - k * startPoint.X;
            double middleBottomX = endPoint.X - Math.Sqrt(triangleHeight + triangleHeight * k * k) / (k * k + 1);
            double middleBottomY = k * endPoint.X + b;

            Console.WriteLine("");
            Point point1 = PointRotate(new PointF((float)endPoint.X, (float)endPoint.Y), new PointF((float)middleBottomX, (float)middleBottomY), 20);
            Point point2 = PointRotate(new PointF((float)endPoint.X, (float)endPoint.Y), new PointF((float)middleBottomX, (float)middleBottomY), -20);

            return new Point[] { endPoint, point1, point2 };
        }

        private Point PointRotate(PointF center, PointF p1, double angle)
        {
            Point tmp = new Point();
            double angleHude = angle * Math.PI / 180;/*角度变成弧度*/
            double x1 = (p1.X - center.X) * Math.Cos(angleHude) + (p1.Y - center.Y) * Math.Sin(angleHude) + center.X;
            double y1 = -(p1.X - center.X) * Math.Sin(angleHude) + (p1.Y - center.Y) * Math.Cos(angleHude) + center.Y;
            tmp.X = (float)x1;
            tmp.Y = (float)y1;
            return tmp;
        }




        private Shape DrawLinkArrow(Point p1, Point p2)
        {
            GeometryGroup lineGroup = new GeometryGroup();
            double theta = Math.Atan2((p2.Y - p1.Y), (p2.X - p1.X)) * 180 / Math.PI;

            PathGeometry pathGeometry = new PathGeometry();
            PathFigure pathFigure = new PathFigure();
            Point p = new Point(p1.X + ((p2.X - p1.X) / 1.35), p1.Y + ((p2.Y - p1.Y) / 1.35));
            pathFigure.StartPoint = p;

            Point lpoint = new Point(p.X + 6, p.Y + 15);
            Point rpoint = new Point(p.X - 6, p.Y + 15);
            LineSegment seg1 = new LineSegment();
            seg1.Point = lpoint;
            pathFigure.Segments.Add(seg1);

            LineSegment seg2 = new LineSegment();
            seg2.Point = rpoint;
            pathFigure.Segments.Add(seg2);

            LineSegment seg3 = new LineSegment();
            seg3.Point = p;
            pathFigure.Segments.Add(seg3);

            pathGeometry.Figures.Add(pathFigure);
            RotateTransform transform = new RotateTransform();
            transform.Angle = theta + 90;
            transform.CenterX = p.X;
            transform.CenterY = p.Y;
            pathGeometry.Transform = transform;

            lineGroup.Children.Add(pathGeometry);


            //直线部分
            LineGeometry connectorGeometry = new LineGeometry();
            connectorGeometry.StartPoint = p1;
            connectorGeometry.EndPoint = p2;
            lineGroup.Children.Add(connectorGeometry);


            System.Windows.Shapes.Path path = new System.Windows.Shapes.Path();
            path.Data = lineGroup;
            path.StrokeThickness = 2;
            path.Stroke = path.Fill = System.Windows.Media.Brushes.Black;

            return path;
        }


        List<TaskItem> allTasks = new List<TaskItem>();

        Dictionary<int, Point> controlPos = new Dictionary<int, Point>();
        private void CreateTask(object sender, RoutedEventArgs e)
        {
            TaskItem taskItem = new TaskItem();

            var pos = Mouse.GetPosition(MainCanvas);
            Canvas.SetLeft(taskItem, pos.X);
            Canvas.SetTop(taskItem, pos.Y);
            controlPos.Add(taskItem.TaskID, pos);
            MainCanvas.Children.Add(taskItem);
            allTasks.Add(taskItem);
        }

        private float currentScale = 1f;
        private void ZoomView(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;
            if (e.Delta > 0)
            {
                currentScale += 0.1f;
            }
            else
            {
                currentScale -= 0.1f;
            }
            if (currentScale < 0.1 || currentScale > 1) return;
            MainCanvas.LayoutTransform = new ScaleTransform(currentScale, currentScale);
        }


    }
}
