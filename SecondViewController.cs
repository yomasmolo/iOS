using System;
using System.IO;
using Foundation;
using UIKit;
using SQLite;
using Geolocator.Plugin;
using MapKit;
using CoreLocation;

namespace ses8_geo
{
	public partial class SecondViewController : UIViewController
	{
		CLLocationManager locationManager;
		double Latitud, Longitud;
		protected SecondViewController(IntPtr handle) : base(handle)
		{
		}

		public override async void ViewDidLoad()
		{
			base.ViewDidLoad();

			var path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			path = Path.Combine(path, "thebase.db3");
			var conn = new SQLiteConnection(path);
			conn.CreateTable<Empleados>();

			Vista.Text = "";
			var fileOrDirectory = Directory.GetFiles
					 (Environment.GetFolderPath
					 (Environment.SpecialFolder.Personal));
			foreach (var entry in fileOrDirectory)
			{
				Vista.Text += entry + Environment.NewLine;
			}


			locationManager = new CLLocationManager();
			locationManager.RequestWhenInUseAuthorization();
			Mapa.ShowsUserLocation = true;
			var locator = CrossGeolocator.Current;
			var position = await locator.GetPositionAsync(10000);
			var Ubicacion = new CLLocation(position.Latitude, position.Longitude);
			var clg = new CLGeocoder();
			var Datos = await clg.ReverseGeocodeLocationAsync(Ubicacion);
			var Pais = Datos[0].Country;
			var Ciudad = Datos[0].Locality;
			MessageBox(Pais, Ciudad);
			Latitud = position.Latitude;
			Longitud = position.Longitude;

			Mapa.MapType = MKMapType.HybridFlyover;
			var Centrar = new CLLocationCoordinate2D(Latitud, Longitud);
			var Altura = new MKCoordinateSpan(.003, .003);
			var Region = new MKCoordinateRegion(Centrar, Altura);
			Mapa.SetRegion(Region, true);



			btnBuscarXML.TouchUpInside += delegate
			{
				string rutaImagen;
				try
				{
					int foliobusca = int.Parse(txtFolioB.Text);
					var elementos = from s in conn.Table
												  <Empleados>()
									where s.Folio == foliobusca
									select s;
					foreach (var fila in elementos)
					{
						txtFolio.Text = fila.Folio.ToString();
						txtNombre.Text = fila.Nombre;
						txtEdad.Text = fila.Edad.ToString();
						txtPuesto.Text = fila.Puesto;
						txtSueldo.Text = fila.Sueldo.ToString();
						rutaImagen = Path.Combine
										 (Environment.GetFolderPath
									  (Environment.SpecialFolder.
									   Personal),
												txtFolioB.Text + ".jpg");
						Imagen.Image = UIImage.FromFile(rutaImagen);

						var RecibeCentrar = new CLLocationCoordinate2D
							(double.Parse(fila.Latitud), double.Parse(fila.Longitud));
						var RecibeAltura = new MKCoordinateSpan(.002, .002);
						var RecibeRegion = new MKCoordinateRegion
							(RecibeCentrar, RecibeAltura);
						Mapa.SetRegion(RecibeRegion, true);
					}
				}
				catch (Exception ex)
				{
					MessageBox("Estatus:", ex.Message);
				}
			};
			txtFolioB.ShouldReturn += (textField) =>
			{
				txtFolioB.ResignFirstResponder();
				return true;
			};
		}
		private void MessageBox(string titulo, string mensaje)
		{
			using (var alerta = new UIAlertView())
			{
				alerta.Title = titulo;
				alerta.Message = mensaje;
				alerta.AddButton("OK");
				alerta.Show();
			}
		}


	}
}