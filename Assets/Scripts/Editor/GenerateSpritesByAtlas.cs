
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public static class 
    GenerateSpritesByAtlas {

    [MenuItem("Assets/Generate Sprites", validate = true)]
    public static bool validate() {
        var obj = Selection.activeObject;
        if (obj == null) {
            return false;
        }
        var path = AssetDatabase.GetAssetPath(obj);
        if (path == null || !path.EndsWith(".atlas")) {
            return false;
        }
        return true;
    }

    [MenuItem("Assets/Generate Sprites")]
    public static void generate() {
        var path = AssetDatabase.GetAssetPath(Selection.activeObject);
        var content = System.IO.File.ReadAllText(path);
        var lines = content.Split('\n');
        if (lines.Length >= 6 && lines[0] == string.Empty) {

            var parent_path = System.IO.Path.GetDirectoryName(path);

            var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(parent_path + "/" + lines[1]);

            if (texture == null) {
                Debug.LogError($"Texture `{lines[1]}` not exist");
            }

            var sprites_folder = parent_path + "/" + "Sprites";
            if (!AssetDatabase.IsValidFolder(sprites_folder)) {
                AssetDatabase.CreateFolder(parent_path, "Sprites");
            }

            var images_folder = parent_path + "/" + "Images";
            if (!AssetDatabase.IsValidFolder(images_folder)) {
                AssetDatabase.CreateFolder(parent_path, "Images");
            }

            for (var index = 6; index < lines.Length; index += 7) {
                var name = lines[index];
                if (name == string.Empty) {
                    break;
                }

                var pos = parse_xy(lines[index + 2]);
                var size = parse_xy(lines[index + 3]);
                var origin_size = parse_xy(lines[index + 4]);
                var offset = parse_xy(lines[index + 5]);

                var rect = new Rect(pos.x, texture.height - (pos.y + size.y), size.x, size.y);

                var sprite = Sprite.Create(
                    texture,
                    rect,
                    new Vector2(0.5f, 0.5f),
                    100, 1, SpriteMeshType.FullRect,
                    Vector4.zero,
                    false);

                var asset_name = get_path_safe_name(name);

                var asset_path = $"{sprites_folder}/{asset_name}.asset";
                AssetDatabase.CreateAsset(sprite, asset_path);

                var prefab_path = $"{images_folder}/{asset_name}.prefab";
                
                var go = new GameObject(asset_name);
                var rt = go.AddComponent<RectTransform>();
                rt.sizeDelta = origin_size;

                var image = new GameObject("[Image]").AddComponent<Image>();
                image.sprite = sprite;
                rt = image.transform as RectTransform;
                rt.SetParent(go.transform, false);
                rt.sizeDelta = size;
                rt.anchorMin = Vector2.up;
                rt.anchorMax = Vector2.up;
                rt.anchoredPosition = new Vector2(offset.x, -offset.y);
                rt.pivot = Vector2.up;


                PrefabUtility.SaveAsPrefabAsset(go, prefab_path);
                Object.DestroyImmediate(go);


                index += 7;
            }
        }


    }

    public static string get_path_safe_name(string name) {
        foreach (char c in System.IO.Path.GetInvalidFileNameChars()) { 
            name = name.Replace(c, '_');
        }
        return name;
    }

    public static Vector2Int parse_xy(string line) {
        line = line.Substring(line.IndexOf(':') + 1);
        var pos = line.IndexOf(',');
        return new Vector2Int(int.Parse(line.Substring(0, pos)), int.Parse(line.Substring(pos + 1)));
    }

}