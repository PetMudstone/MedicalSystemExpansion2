﻿using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace OrenoMSE
{
    class CompIncludedChildParts : ThingComp
    {
        public override void PostPostMake ()
        {
            base.PostPostMake();
            //Log.Message( "CompPostMake" );

            if ( this.Props.standardChildren != null )
            {
                foreach ( ThingDef sChild in this.Props.standardChildren )
                {
                    //Log.Message( "Adding from def: " + sChild.defName );
                    this.childPartsIncluded.Add( ThingMaker.MakeThing( sChild ) );
                }
            }
        }

        public override void PostDestroy ( DestroyMode mode, Map previousMap )
        {
            base.PostDestroy( mode, previousMap );
            if ( this.childPartsIncluded != null )
            {
                foreach ( ThingWithComps childPart in this.childPartsIncluded )
                {
                    childPart.Destroy( DestroyMode.Vanish );
                }
            }
        }
        public override string CompInspectStringExtra ()
        {
            if ( this.childPartsIncluded != null )
            {
                //StringBuilder stringBuilder = new StringBuilder();
                //for ( int i = 0; i < this.childPartsIncluded.Count; i++ )
                //{
                //    stringBuilder.AppendLine( this.childPartsIncluded[i].Label );
                //}
                //return stringBuilder.ToString().TrimEndNewlines();
                return "Includes " + this.childPartsIncluded.Count + ( this.childPartsIncluded.Count > 1 ? " subparts" : " subpart" );
            }
            return null;
        }
        public CompProperties_CompIncludedChildParts Props
        {
            get
            {
                return (CompProperties_CompIncludedChildParts) this.props;
            }
        }
        public override IEnumerable<StatDrawEntry> SpecialDisplayStats ()
        {
            if ( this.childPartsIncluded != null )
            {
                List<Dialog_InfoCard.Hyperlink> partLinks = new List<Dialog_InfoCard.Hyperlink>();
                foreach ( Thing childPart in this.childPartsIncluded )
                {
                    partLinks.Add( new Dialog_InfoCard.Hyperlink( childPart ) );
                }

                List<Dialog_InfoCard.Hyperlink> missingPartLinks = new List<Dialog_InfoCard.Hyperlink>();
                foreach ( ThingDef childPart in this.MissingParts() )
                {
                    missingPartLinks.Add( new Dialog_InfoCard.Hyperlink( childPart ) );
                }

                yield return new StatDrawEntry(
                    StatCategoryDefOf.Basics,
                    "Included subparts:", // Translate
                    partLinks.Count.ToString(),
                    "When implanted it will also install theese:",
                    2500,
                    null,
                    partLinks,
                    false );

                if ( missingPartLinks.Count > 0 )
                {
                    yield return new StatDrawEntry(
                        StatCategoryDefOf.Basics,
                        "Missing subparts:", // Translate
                        partLinks.Count.ToString(),
                        "These parts are missing:",
                        2501,
                        null,
                        missingPartLinks,
                        false );
                }

            }
            yield break;
        }
        public override void PostExposeData ()
        {
            base.PostExposeData();
            Scribe_Collections.Look<Thing>( ref this.childPartsIncluded, "childPartsIncluded", LookMode.Deep );
        }

        public IEnumerable<ThingDef> MissingParts()
        {
            if ( this.props != null )
            {
                List< ThingDef > included = this.childPartsIncluded.ConvertAll((Thing x) => { return x.def; });
            
                foreach ( ThingDef expectedDef in this.Props.standardChildren )
                {
                    if ( included.Contains( expectedDef ) )
                    {
                        included.Remove( expectedDef );
                    }
                    else
                    {
                        yield return expectedDef;
                    }
                }
            }

            yield break;
        }

        public List<Thing> childPartsIncluded = new List<Thing>();
    }

    class CompProperties_CompIncludedChildParts : CompProperties
    {
        public CompProperties_CompIncludedChildParts()
        {
            this.compClass = typeof( CompIncludedChildParts );
        }

        public override void ResolveReferences ( ThingDef parentDef )
        {
            base.ResolveReferences( parentDef );

            // autogen
            if (autogenerate)
            {
                string name = parentDef.defName;

                if ( name.Contains("Arm") )
                {
                    this.standardChildren.Add( DefDatabase<ThingDef>.GetNamed( name.Replace( "Arm", "Hand" ), false ) );
                    this.standardChildren.Add( DefDatabase<ThingDef>.GetNamed( name.Replace( "Arm", "Humerus" ), false ) );
                    this.standardChildren.Add( DefDatabase<ThingDef>.GetNamed( name.Replace( "Arm", "Radius" ), false ) );
                }
                if ( name.Contains( "Hand" ) )
                {
                    this.standardChildren.Add( DefDatabase<ThingDef>.GetNamed( name.Replace( "Hand", "Finger" ), false ) );
                    this.standardChildren.Add( DefDatabase<ThingDef>.GetNamed( name.Replace( "Hand", "Finger" ), false ) );
                    this.standardChildren.Add( DefDatabase<ThingDef>.GetNamed( name.Replace( "Hand", "Finger" ), false ) );
                    this.standardChildren.Add( DefDatabase<ThingDef>.GetNamed( name.Replace( "Hand", "Finger" ), false ) );
                    this.standardChildren.Add( DefDatabase<ThingDef>.GetNamed( name.Replace( "Hand", "Finger" ), false ) );
                }

                if ( name.Contains( "Leg" ) )
                {
                    this.standardChildren.Add( DefDatabase<ThingDef>.GetNamed( name.Replace( "Leg", "Foot" ), false ) );
                    this.standardChildren.Add( DefDatabase<ThingDef>.GetNamed( name.Replace( "Leg", "Femur" ), false ) );
                    this.standardChildren.Add( DefDatabase<ThingDef>.GetNamed( name.Replace( "Leg", "Tibia" ), false ) );
                }
                if ( name.Contains( "Foot" ) )
                {
                    this.standardChildren.Add( DefDatabase<ThingDef>.GetNamed( name.Replace( "Foot", "Toe" ), false ) );
                    this.standardChildren.Add( DefDatabase<ThingDef>.GetNamed( name.Replace( "Foot", "Toe" ), false ) );
                    this.standardChildren.Add( DefDatabase<ThingDef>.GetNamed( name.Replace( "Foot", "Toe" ), false ) );
                    this.standardChildren.Add( DefDatabase<ThingDef>.GetNamed( name.Replace( "Foot", "Toe" ), false ) );
                    this.standardChildren.Add( DefDatabase<ThingDef>.GetNamed( name.Replace( "Foot", "Toe" ), false ) );
                }
            }

        }



        public List<ThingDef> standardChildren = new List<ThingDef>();

        public bool autogenerate = false;
    }

}
