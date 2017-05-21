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
	public partial class FirstViewController : UIViewController
	{
		string jpgFilename, ruta;
		private UIImagePickerController SeleccionadorImagen;
		CLLocationManager locationManager;
		double Latitud, Longitud;

		protected FirstViewController(IntPtr handle) : base(handle)
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
			SeleccionadorImagen = new UIImagePickerController();
			SeleccionadorImagen.FinishedPickingMedia += SeleccionImagen;
			SeleccionadorImagen.Canceled += ImagenCancelada;
			SeleccionadorImagen.AllowsEditing = true;
			if (UIImagePickerController.IsSourceTypeAvailable
				(UIImagePickerControllerSourceType.Camera))
			{
				SeleccionadorImagen.SourceType =
					UIImagePickerControllerSourceType.Camera;
			}
			else
			{
				SeleccionadorImagen.SourceType =
					UIImagePickerControllerSourceType.PhotoLibrary;
			}
			btnImagen.TouchUpInside += delegate
			{
				PresentViewController(SeleccionadorImagen, true, null);
			};

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



			btnGuardarXML.TouchUpInside += delegate
			{
				try
				{
					var Insertar = new Empleados();
					Insertar.Folio = int.Parse(txtFolio.Text);
					Insertar.Nombre = txtNombre.Text;
					Insertar.Edad = int.Parse(txtEdad.Text);
					Insertar.Puesto = txtPuesto.Text;
					Insertar.Sueldo = double.Parse(txtSueldo.Text);
					Insertar.Foto = txtFolio.Text + ".jpg";
					Insertar.Latitud = Latitud.ToString();
					Insertar.Longitud = Longitud.ToString();
					Insertar.Pais = Pais.ToString();
					Insertar.Localidad = Ciudad.ToString();
					conn.Insert(Insertar);
					txtFolio.Text = "";
					txtNombre.Text = "";
					txtEdad.Text = "";
					txtPuesto.Text = "";
					txtSueldo.Text = "";
					MessageBox("Guardado Correctamente", "SQLite");
				}
				catch (Exception ex)
				{
					MessageBox("Estatus:", ex.Message);
				}
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
		private void SeleccionImagen(object sender,
			UIImagePickerMediaPickedEventArgs e)
		{
			try
			{
				var ImagenSeleccionada = e.Info
					[UIImagePickerController.OriginalImage] as UIImage;
				var rutaImagen = Path.Combine(Environment.GetFolderPath
						(Environment.SpecialFolder.Personal),
											  txtFolio.Text + ".jpg");
				if (File.Exists(rutaImagen))
				{
					MessageBox("Error", "Imagen ya existente");
					SeleccionadorImagen.DismissViewController(true, null);
				}
				else
				{
					ruta = Environment.GetFolderPath
							(Environment.SpecialFolder.Personal);
					jpgFilename = Path.Combine(ruta, txtFolio.Text + ".jpg");
					NSError err;
					var imgData = ImagenSeleccionada.AsJPEG();
					imgData.Save(jpgFilename, false, out err);
					Imagen.Image = UIImage.FromFile(jpgFilename);
					SeleccionadorImagen.DismissViewController(true, null);
				}
			}
			catch (Exception ex)
			{
				MessageBox("Error", ex.Message);
				SeleccionadorImagen.DismissViewController(true, null);
			}
		}
		private void ImagenCancelada(object sender, EventArgs e)
		{
			SeleccionadorImagen.DismissViewController(true, null);
		}
	}
	public class Empleados
	{
		public int Folio { get; set; }
		public string Nombre { get; set; }
		public int Edad { get; set; }
		public string Puesto { get; set; }
		public double Sueldo { get; set; }
		public string Foto { get; set; }
		public string Latitud { get; set; }
		public string Longitud { get; set; }
		public string Pais { get; set; }
		public string Localidad { get; set; }
	}
}
