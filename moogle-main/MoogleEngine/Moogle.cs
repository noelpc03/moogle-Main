namespace MoogleEngine;

public static class Moogle
{
    public static SearchResult Query(string query)
    {
        Initializer.Search(query); // Realizacion de la busqueda
        
        // Segun la cantidad de textos relevantes se crean los objetos correspondientes
        if (Initializer.Count == 0) 
        {
            SearchItem[] items = new SearchItem[1] {
            new SearchItem("No se encontraron documetos :(","",0f)};
            return new SearchResult(items, query);
        }
        else if (Initializer.Count == 1)
        {
            SearchItem[] items = new SearchItem[1]{
            new SearchItem(Exit.GetName(Initializer.TextAndScore.Keys.ElementAt(0)), Exit.Snippet(Initializer.TextAndScore.Keys.ElementAt(0),Initializer.Query) , 0.9f)};
            return new SearchResult(items, query);
        }
        else if (Initializer.Count == 2)
        {
            SearchItem[] items = new SearchItem[2] {
            new SearchItem(Exit.GetName(Initializer.TextAndScore.Keys.ElementAt(0)),  Exit.Snippet(Initializer.TextAndScore.Keys.ElementAt(0),Initializer.Query) , 0.9f),
            new SearchItem(Exit.GetName(Initializer.TextAndScore.Keys.ElementAt(1)),  Exit.Snippet(Initializer.TextAndScore.Keys.ElementAt(1),Initializer.Query), 0.7f)};
            return new SearchResult(items, query);
        }
        else
        {
            SearchItem[] items = new SearchItem[3] {
            new SearchItem(Exit.GetName(Initializer.TextAndScore.Keys.ElementAt(0)),  Exit.Snippet(Initializer.TextAndScore.Keys.ElementAt(0),Initializer.Query), 0.9f),
            new SearchItem(Exit.GetName(Initializer.TextAndScore.Keys.ElementAt(1)),  Exit.Snippet(Initializer.TextAndScore.Keys.ElementAt(1),Initializer.Query), 0.7f),
            new SearchItem(Exit.GetName(Initializer.TextAndScore.Keys.ElementAt(2)),  Exit.Snippet(Initializer.TextAndScore.Keys.ElementAt(2),Initializer.Query), 0.5f)};
            return new SearchResult(items, query);
        }
    }
}