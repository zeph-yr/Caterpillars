using HarmonyLib;
using System;
using System.Collections.Generic;
using Worms.Configuration;

namespace Worms
{
    [HarmonyPatch(typeof(BeatmapDataTransformHelper), "CreateTransformedBeatmapData")]
    internal class BeatmapDataTransformPatch
    {
        static IReadonlyBeatmapData Postfix(IReadonlyBeatmapData __result)
        {
            Plugin.Log.Debug("Worms: Prefix");

            if (PluginConfig.Instance.enabled == false)
            {
                return __result;
            }


            BeatmapData result_2 = new BeatmapData(__result.numberOfLines);

            bool last_was_head = false;

            NoteData noteData;
            NoteData prev_noteData = null;
            int i = 0;

            foreach (BeatmapDataItem beatmapDataItem in __result.allBeatmapDataItems)
            {
                //Plugin.Log.Debug("Worms: " + i);

                if ((noteData = (beatmapDataItem as NoteData)) != null && noteData.colorType != ColorType.None) // && noteData.cutDirection != NoteCutDirection.Any) 
                {
                    if (i == 0)
                    {
                        prev_noteData = noteData;
                        last_was_head = true;
                    }

                    else if (last_was_head == false)
                    {
                        prev_noteData = NoteData.CreateBasicNoteData(noteData.time, noteData.lineIndex, noteData.noteLineLayer, noteData.colorType, noteData.cutDirection);
                        
                        //noteData.ChangeToBurstSliderHead();
                        //result_2.AddBeatmapObjectData(noteData);

                        last_was_head = true;
                    }

                    else //if (prev_noteData != null && prev_noteData.cutDirection != NoteCutDirection.Any)
                    {
                        SliderData new_slider = SliderData.CreateBurstSliderData(prev_noteData.colorType, prev_noteData.time, prev_noteData.lineIndex, prev_noteData.noteLineLayer, prev_noteData.beforeJumpNoteLineLayer, prev_noteData.cutDirection, noteData.time, noteData.lineIndex, noteData.noteLineLayer, noteData.beforeJumpNoteLineLayer, noteData.cutDirection, (int)PluginConfig.Instance.links_value, PluginConfig.Instance.squish_value);
                        result_2.AddBeatmapObjectData(new_slider);

                        last_was_head = false;
                    }

                    prev_noteData = noteData;
                    i++;
                }


                ObstacleData obstacleData;
                if ((obstacleData = (beatmapDataItem as ObstacleData)) != null)
                {
                    result_2.AddBeatmapObjectData(obstacleData);
                }

                BeatmapEventData beatmapEventData;
                if ((beatmapEventData = (beatmapDataItem as BeatmapEventData)) != null)
                {
                    result_2.InsertBeatmapEventData(beatmapEventData);
                }

            }

            foreach (string keyword in __result.specialBasicBeatmapEventKeywords)
            {
                result_2.AddSpecialBasicBeatmapEventKeyword(keyword);
            }


            return result_2;
        }
    }
}