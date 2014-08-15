///////////////////////////////////////////////////////////////////////////////////////////
// StageFactory.cs
//
//
//
//       --------------------------------------------------------------------------
//       |                     |                        |                         |
//       |       Left          |        Center          |        Right            |
//       |     Background      |      Background        |      Background         |
//       |                     |                        |                         |
//       |                     |                        |                         |
//       |                     |                        |                         |
//       --------------------------------------------------------------------------
//      /                      /                        \                         \
//     /       Left           /         Center           \         Right           \
//    /       Floor          /          Floor             \        Floor            \
//   /                      /       (Game Play Area)       \                         \
//  /                      /                                \                         \
//  ------------------------------------------------------------------------------------
//
//
// <stage cellSize="50" backgroundHeight="100" floorHeight="100">
//     <left width="50">
//         <background texture="Backgrounds/sky" row="0" col="0" zOrder="100"/>
//         <background texture="Backgrounds/city" row="0" col="0" zOrder="0"/>
//         <floor texture="Backgrounds/grass" row="0" col="0"/>
//     </left>
//     <center width="200">vv
//         <background texture="Backgrounds/sky" row="0" col="0" zOrder="100"/>
//         <background texture="Backgrounds/city" row="0" col="0" zOrder="0"/>
//         <floor texture="Backgrounds/grass" row="0" col="0"/>
//     </center>
//     <right width="50">
//         <background texture="Backgrounds/sky" row="0" col="0" zOrder="100"/>
//         <background texture="Backgrounds/city" row="0" col="0" zOrder="0"/>
//         <floor texture="Backgrounds/grass" row="0" col="0"/>
//     </right>
// </stage>
//
// Copyright (C) 2011 Lunch Hour Games. All rights reserved.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using LunchHourGames.Screen;

namespace LunchHourGames.Stage
{
    public class StageFactory
    {
        private LunchHourGames lhg;
        private LoadingProgress loadingProgress;

        public StageFactory(LunchHourGames lhg, LoadingProgress loadingProgress)
        {
            this.lhg = lhg;
            this.loadingProgress = loadingProgress;
        }

        public LHGStage loadStage(XmlNode stageNode)
        {
            LHGStage stage = null;
            XmlNodeList stageChildren = stageNode.ChildNodes;
            foreach (XmlNode childNode in stageChildren)
            {
                switch (childNode.Name)
                {
                    case "dimensions":
                        int cellSize = 0, backgroundHeight = 0, floorHeight = 0, leftWidth = 0, centerWidth = 0, rightWidth = 0;
                        if (loadDimensions(childNode, ref cellSize, ref backgroundHeight, ref floorHeight, ref leftWidth, ref centerWidth, ref rightWidth))
                        {
                            stage = new LHGStage(this.lhg, cellSize, backgroundHeight, floorHeight, leftWidth, centerWidth, rightWidth);
                        }
                        break;
                    
                    case "left":
                        if (stage != null)
                            loadSection(stage, LHGStage.StageSection.Left, childNode);
                        break;

                    case "center":
                        if (stage != null)
                            loadSection(stage, LHGStage.StageSection.Center, childNode);
                        break;

                    case "right":
                        if (stage != null)
                            loadSection(stage, LHGStage.StageSection.Right, childNode);
                        break;
                }
            }

            return stage;
        }

        private bool loadDimensions(XmlNode dimensionsNode, ref int cellSize, ref int backgroundHeight,
                                    ref int floorHeight, ref int leftWidth, ref int centerWidth, ref int rightWidth)
        {
            if (dimensionsNode != null)
            {
                try
                {
                    cellSize = Convert.ToInt16(dimensionsNode.Attributes["cellSize"].Value);
                    backgroundHeight = Convert.ToInt16(dimensionsNode.Attributes["backgroundHeight"].Value);
                    floorHeight = Convert.ToInt16(dimensionsNode.Attributes["floorHeight"].Value);
                    leftWidth = Convert.ToInt16(dimensionsNode.Attributes["leftWidth"].Value);
                    centerWidth = Convert.ToInt16(dimensionsNode.Attributes["centerWidth"].Value);
                    rightWidth = Convert.ToInt16(dimensionsNode.Attributes["rightWidth"].Value);
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("StageFactory.loadDimensions - " + ex.Message);
                }

                return true;
            }

            return false;
        }

        private void loadSection(LHGStage stage, LHGStage.StageSection section, XmlNode sectionNode)
        {
            XmlNodeList sectionChildren = sectionNode.ChildNodes;
            foreach (XmlNode childNode in sectionChildren)
            {
                switch (childNode.Name)
                {
                    case "background":
                        loadBackground(stage, section, childNode);
                        break;

                    case "floor":
                        loadFloor(stage, section, childNode);
                        break;
                }
            }
        }

        private void loadBackground(LHGStage stage, LHGStage.StageSection position, XmlNode childNode)
        {
            string texture = childNode.Attributes["texture"].Value;
            int i = Convert.ToInt16(childNode.Attributes["row"].Value);
            int j = Convert.ToInt16(childNode.Attributes["col"].Value);
            int zOrder = Convert.ToInt16(childNode.Attributes["zOrder"].Value);

            stage.addBackground(position, texture, i, j, zOrder);
        }

        private void loadFloor(LHGStage stage, LHGStage.StageSection position, XmlNode childNode)
        {
            string texture = childNode.Attributes["texture"].Value;
            int i = Convert.ToInt16(childNode.Attributes["row"].Value);
            int j = Convert.ToInt16(childNode.Attributes["col"].Value);

            stage.addFloor(position, texture, i, j);
        }
    }
}
