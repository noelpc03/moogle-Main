namespace MoogleEngine;


public static class Moogle
{
    public static SearchResult Quer(string query) {
        Ejecution.Part2(query);

         
        
        if (Exit.Count==0)
        {
             SearchItem [] items = new SearchItem[1] {
             new SearchItem("No se encontraron documetos",":(",0f)}; 
            return new SearchResult(items, query);
              
        }
         
         else if (Exit.Count==1)
        {
            SearchItem [] items = new SearchItem[1]{
            new SearchItem(Vocabulary.files[Exit.PositionMax(Vocabulary.Texts)], Exit.GetSubstringWithWordAtPosition(Vocabulary.Texts[Exit.PositionMax(Vocabulary.Texts)].Item1,Query.QueryModified) , 0.9f)};
            return new SearchResult(items, query);
        }
         if (Exit.Count==2)
        {
            SearchItem [] items = new SearchItem[2] {
            new SearchItem(Vocabulary.files[Exit.PositionMax(Vocabulary.Texts)],  Exit.GetSubstringWithWordAtPosition(Vocabulary.Texts[Exit.PositionMax(Vocabulary.Texts)].Item1,Query.QueryModified) , 0.9f),
            new SearchItem(Vocabulary.files[Exit.PositionSecondMax(Vocabulary.Texts)],  Exit.GetSubstringWithWordAtPosition(Vocabulary.Texts[Exit.PositionSecondMax(Vocabulary.Texts)].Item1,Query.QueryModified), 0.7f)};
             return new SearchResult(items, query);
        }
        else {
            SearchItem [] items = new SearchItem[3] {
            new SearchItem(Vocabulary.files[Exit.PositionMax(Vocabulary.Texts)],  Exit.GetSubstringWithWordAtPosition(Vocabulary.Texts[Exit.PositionMax(Vocabulary.Texts)].Item1,Query.QueryModified), 0.9f),
            new SearchItem(Vocabulary.files[Exit.PositionSecondMax(Vocabulary.Texts)],  Exit.GetSubstringWithWordAtPosition(Vocabulary.Texts[Exit.PositionSecondMax(Vocabulary.Texts)].Item1,Query.QueryModified), 0.7f),
            new SearchItem(Vocabulary.files[Exit.PositionThirdMax(Vocabulary.Texts)],  Exit.GetSubstringWithWordAtPosition(Vocabulary.Texts[Exit.PositionThirdMax(Vocabulary.Texts)].Item1,Query.QueryModified), 0.5f)};
        
        
        return new SearchResult(items, query);
        }
    }
}
