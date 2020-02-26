Secrets are in the same folder as this document, as secrets.json.

I've used Code First, with seeding done by the DbInitializer class. Run Migrations and run the program to get the full database.

Some Migrations are blank, I decided not to delete them in case something broke (has happened to me before).

I have attempted all sections of this assignemt.


HD (Point 10) Section:
	Administrator User Name: admin@ABCCC.com
	Administrator Password : Im_th3_Admin

These constants are located in DbInitializer as UserName and Password
Google authentication is not used with this admin account.

Image refs:

All Movie posters: IMDb
Cineplex Maps: Google Maps
Banner 1: https://www.phoenix.org.uk/about-phoenix/about-cinema/
Banner 2: https://pixabay.com/en/banner-header-film-cinema-video-1155437/
Banner 3: http://rialtocommunityplayers.com/?page_id=55 (background image)

Port number is: 44384, referenced in SessionsContoller, site.js, and startup