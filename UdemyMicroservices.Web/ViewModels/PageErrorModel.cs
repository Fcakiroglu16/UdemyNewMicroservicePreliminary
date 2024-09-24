namespace UdemyMicroservices.Web.ViewModels
{
    public record PageErrorModel
    {
        public string? Title { get; private set; }
        public string? Description { get; private set; }

        public PageErrorModel(string? title, string? description)
        {
            SetTitle(title);
            SetDescription(description);
        }

        public PageErrorModel()
        {
        }

        public bool HasDescription => !string.IsNullOrEmpty(Description);


        public void SetTitle(string? title)
        {
            if (!string.IsNullOrEmpty(title))
            {
                Title = title;
            }
        }

        public void SetDescription(string? description)
        {
            if (!string.IsNullOrEmpty(description))
            {
                {
                    Description = description;
                }
            }
        }
    }
}