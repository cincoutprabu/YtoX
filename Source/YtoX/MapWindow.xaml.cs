//ChooseLocation.xaml.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using Microsoft.Maps.MapControl.WPF;

using YtoX.Entities;
using YtoX.Core;

namespace YtoX
{
    public partial class MapWindow : Window
    {
        #region Properties

        public Position Position { get; set; }
        public event Action<Position> Chosen;

        #endregion

        #region Constructors

        public MapWindow()
        {
            InitializeComponent();

            //MainMap.ViewChangeOnFrame += MainMap_ViewChangeOnFrame;
            MainMap.MouseDoubleClick += MainMap_MouseDoubleClick;
        }

        public MapWindow(Position position)
            : this()
        {
            this.Position = position;

            if (this.Position.IsValid())
            {
                MainMap.Center = new Location(this.Position.Latitude, this.Position.Longitude);
                MainMap.ZoomLevel = 16;

                Pushpin pin = new Pushpin();
                pin.Location = MainMap.Center;
                MainMap.Children.Add(pin);
            }
        }

        #endregion

        #region Control-Events

        void MainMap_ViewChangeOnFrame(object sender, MapEventArgs e)
        {
            this.Position.Latitude = MainMap.Center.Latitude;
            this.Position.Longitude = MainMap.Center.Longitude;
        }

        private void MainMap_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Point p = e.GetPosition(this);
            Location location = MainMap.ViewportPointToLocation(p);

            Position position = new Position()
            {
                Key = this.Position.Key,
                Latitude = location.Latitude,
                Longitude = location.Longitude,
                FetchedOn = DateTime.Now
            };
            if (Chosen != null)
            {
                Chosen(position);
                this.Close();
            }
        }

        #endregion

    }
}
