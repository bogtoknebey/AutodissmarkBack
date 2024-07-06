namespace Autodissmark.Domain.ApplicationModels;

public class TextModel
{
    public int Id { get; set; }
    public int AuthorEntityId { get; set; }
    public string Text { get; set; }
    public string Title { get; set; }
    public DateTime AddedDate { get; set; }

    public static TextModel Create(int authorEntityId, string text, string title, DateTime addedDate)
    {
        return new TextModel()
        {
            AuthorEntityId = authorEntityId,
            Text = text,
            Title = title,
            AddedDate = addedDate
        };
    }
}
