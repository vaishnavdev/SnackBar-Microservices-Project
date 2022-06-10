namespace SnackBar.Web
{
    public static class StaticDetails
    {
        public static String ProductAPIBase { get; set; }
        public static String ShoppingCartAPIBase { get; set; }

        public static String CouponAPIBase { get; set; }
        public enum ApiType {
          GET, POST, PUT, DELETE
        }
    }
}
