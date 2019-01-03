
using App.Model;

namespace App.Util.Cacher
{
    public class FileCharacterCacher : CacherBase<FileCharacterCacher, App.Model.File.MCharacter>
    {
        public Model.File.MCharacter GetFromCharacterId(int characterId)
        {
            if (datas != null && dictionary.Count == 0)
            {
                System.Array.ForEach(datas, child => {
                    dictionary.Add(child.characterId, child);
                });
            }
            Model.File.MCharacter fileCharacter = dictionary.ContainsKey(characterId) ? dictionary[characterId] : null;
            if (fileCharacter != null) {
                return fileCharacter;
            }
            fileCharacter = new Model.File.MCharacter();
            fileCharacter.characterId = characterId;
            //fileCharacter.weapon = 1;
            //fileCharacter.horse = 1;
            //fileCharacter.clothes = 1;
            fileCharacter.level = 1;
            return fileCharacter;
        }
    }
}