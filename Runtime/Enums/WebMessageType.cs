namespace ReupVirtualTwin.enums
{
    public class WebMessageType
    {
        public const string showMaterialsOptions = "showMaterialsOptions";
        public const string hideMaterialsOptions = "hideMaterialsOptions";

        public const string setEditMode = "[Edit Mode] Set Edit Mode";
        public const string setEditModeSuccess = "[Edit Mode] Set Edit Mode Success";

        public const string setSelectedObjects = "[Selected Objects] Set Selected Objects Success v2";
        public const string clearSelectedObjects = "[Selected Objects] Clear Selected Objects";
        public const string allowSelection = "[Selected Objects] Allow Selection";
        public const string disableSelection = "[Selected Objects] Disable Selection";

        public const string activatePositionTransform = "[Transform Objects] Activate Position Transform Mode";
        public const string activatePositionTransformSuccess = "[Transform Objects] Activate Position Transform Mode Success";
        public const string activateRotationTransform = "[Transform Objects] Activate Rotation Transform Mode";
        public const string activateRotationTransformSuccess = "[Transform Objects] Activate Rotation Transform Mode Success";
        public const string deactivateTransformMode = "[Transform Objects] Deactivate Transform Mode";
        public const string deactivateTransformModeSuccess = "[Transform Objects] Deactivate Transform Mode Success";

        public const string deleteObjects = "[Delete Objects] Delete Objects";
        public const string deleteObjectsSuccess = "[Delete Objects] Delete Objects Success";

        public const string loadObject = "[Load Objects] Load Object";
        public const string loadObjectSuccess = "[Load Objects] Load Object Success v2";
        public const string loadObjectProcessUpdate = "[Load Objects] Load Object Process Update";

        public const string changeObjectColor = "[Change Color] Change Object Color";
        public const string changeObjectColorSuccess = "[Change Color] Change Object Color Success";

        public const string changeObjectsMaterial = "[Change Material] Change Object Material";
        public const string changeObjectsMaterialSuccess = "[Change Material] Change Object Material Success";
        public const string changeObjectsMaterialFailure = "[Change Material] Change Object Material Failure";

        public const string error = "[Error] Engine Error";

        public const string requestModelInfo = "[Initial Load Request] Request ModelInfo";
        public const string requestModelInfoSuccess = "[Initial Load Request] Request ModelInfo Success v2";

        public const string updateBuilding = "[Update Building] Update Building Success";

        public const string requestSceneState = "[Load Save Scene] Get Scene State";
        public const string requestSceneStateSuccess = "[Load Save Scene] Get Scene State Success";
        public const string requestSceneLoad = "[Load Save Scene] Load Scene";
        public const string requestSceneLoadSuccess = "[Load Save Scene] Load Scene Success";
        public const string requestSceneLoadFailure = "[Load Save Scene] Load Scene Failure";

        public const string activateViewMode = "[View Mode] Activate View Mode";
        public const string activateViewModeSuccess = "[View Mode] Activate View Mode Success";
        public const string activateViewModeFailure = "[View Mode] Activate ViewMode Failure";

        public const string slideToSpace = "[Navigation] Slide To Space";
        public const string slideToSpaceSuccess = "[Navigation] Slide To Space Success";
        public const string slideToSpaceInterrupted = "[Navigation] Slide To Space Interrupted";
        public const string slideToSpaceFailure = "[Navigation] Slide To Space Failure";

        public const string showObjects = "[Visibility] Show Objects";
        public const string hideObjects = "[Visibility] Hide Objects";
        public const string showAllObjects = "[Visibility] Show All Objects";
        public const string showHideObjectsSuccess = "[Visibility] Show/Hide Objects Success";
        public const string showHideObjectsFailure = "[Visibility] Show/Hide Objects Failure";

        public const string requestObjectTagsUnderCharacter = "[Model Info] Request Object Tags Under Character";
        public const string requestObjectTagsUnderCharacterSuccess = "[Model Info] Request Object Tags Under Character Success";
        public const string requestObjectTagsUnderCharacterFailure = "[Model Info] Request Object Tags Under Character Failure";

    }
}
