
using App.Model;

namespace App.Util.Cacher
{
    public class FileCharacterCacher : CacherBase<FileCharacterCacher, App.Model.File.MCharacter>
    {
        public Model.File.MCharacter GetFromCharacterId(int characterId)
        {
            if (dictionary.Count == 0)
            {
                System.Array.ForEach(datas, child => {
                    dictionary.Add(child.characterId, child);
                });
            }
            return dictionary[characterId];
        }
    }
}