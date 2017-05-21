// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace ses8_geo
{
	[Register ("FirstViewController")]
	partial class FirstViewController
	{
		[Outlet]
		UIKit.UIButton btnGuardarXML { get; set; }

		[Outlet]
		UIKit.UIButton btnImagen { get; set; }

		[Outlet]
		UIKit.UIImageView Imagen { get; set; }

		[Outlet]
		MapKit.MKMapView Mapa { get; set; }

		[Outlet]
		UIKit.UITextField txtEdad { get; set; }

		[Outlet]
		UIKit.UITextField txtFolio { get; set; }

		[Outlet]
		UIKit.UITextField txtNombre { get; set; }

		[Outlet]
		UIKit.UITextField txtPuesto { get; set; }

		[Outlet]
		UIKit.UITextField txtSueldo { get; set; }

		[Outlet]
		UIKit.UITextView Vista { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (btnGuardarXML != null) {
				btnGuardarXML.Dispose ();
				btnGuardarXML = null;
			}

			if (btnImagen != null) {
				btnImagen.Dispose ();
				btnImagen = null;
			}

			if (Imagen != null) {
				Imagen.Dispose ();
				Imagen = null;
			}

			if (Mapa != null) {
				Mapa.Dispose ();
				Mapa = null;
			}

			if (txtEdad != null) {
				txtEdad.Dispose ();
				txtEdad = null;
			}

			if (txtFolio != null) {
				txtFolio.Dispose ();
				txtFolio = null;
			}

			if (txtNombre != null) {
				txtNombre.Dispose ();
				txtNombre = null;
			}

			if (txtPuesto != null) {
				txtPuesto.Dispose ();
				txtPuesto = null;
			}

			if (txtSueldo != null) {
				txtSueldo.Dispose ();
				txtSueldo = null;
			}

			if (Vista != null) {
				Vista.Dispose ();
				Vista = null;
			}
		}
	}
}
