using UnityEngine;

namespace Utilies
{
    public class Size
    {
        public static void newScale(GameObject theGameObject, float newSize) {
            var size = theGameObject.GetComponent<Renderer> ().bounds.size.y;
            var rescale = theGameObject.transform.localScale;
            rescale *= newSize/size; 
            theGameObject.transform.localScale = rescale;
        }
    }
}