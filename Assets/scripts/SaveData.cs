public interface SaveData
{
    /**
    * Save Deliminator Hierarchy
    *
    *   ;
    *   /
    *   :
    *   %
    *   ,
    *
    *
    **/

    string save();

    void load(string s);
    
}