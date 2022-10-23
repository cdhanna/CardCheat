using System;
using System.Collections;
using System.Collections.Generic;
using BrewedInk.CardCheat.Game;
using UnityEngine;
using UnityEngine.UI;

public class GameViewRendererBehaviour : MonoBehaviour
{
   public RenderTexture gameView;
   public RawImage image;
   
   private int rawImageOriginWidth;
   private int rawImageOriginHeight;
   private RectTransform imageTransform;

   void Start()
   {
      imageTransform = image.GetComponent<RectTransform>();

   }

   private void Update()
   {
      image.texture = gameView;

      // https://answers.unity.com/questions/1680940/rawimage-with-correct-aspect-ratio.html
      rawImageOriginWidth = Mathf.RoundToInt(imageTransform.rect.width);
      rawImageOriginHeight = Mathf.RoundToInt(imageTransform.rect.height);
      var scaledVideo = scaleResolution(gameView.width, gameView.height, rawImageOriginWidth, rawImageOriginHeight);
      
      imageTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, scaledVideo[0]);
      imageTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, scaledVideo[1]);

   }

   int[] scaleResolution(int width, int heigth, int maxWidth, int maxHeight)
   {
      int new_width = width;
      int new_height = heigth;
 
      if (width > heigth){
         new_width = maxWidth;
         new_height = (new_width * heigth) / width;
      }
      else
      {
         new_height = maxHeight;
         new_width = (new_height * width) / heigth;
      }
 
      int[] dimension = { new_width, new_height };
      return dimension;
   }
}
