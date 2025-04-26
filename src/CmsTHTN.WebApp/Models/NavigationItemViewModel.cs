namespace CmsTHTN.WebApp.Models
{
    public class NavigationItemViewModel
    {
        public string Slug { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public int SortOrder { get; set; }
        public List<NavigationItemViewModel> Children { get; set; } = new List<NavigationItemViewModel>();

        public bool HasChildren
        {
            get
            {
                return Children.Count > 0;
            }
        }
    }
}
