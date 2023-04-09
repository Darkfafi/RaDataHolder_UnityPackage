# Changelog RaDataHolder

## v1.4.0 - 09/04/2023
* Fully implmented ReplaceData feature 
  * Added to RaDataHolderCore, RaDataHolderBase & RaMonoDataHolderBase
  * Introduced new event called "DataReplacedEvent"
  * Introduced new boolean called "IsReplacingData" which is true while data is being replaced (can be read during ClearData & SetData Callbacks / Events to determine behaviour)

## v1.3.3 - 18/03/2023
* Made it so the Resolve boolean's default value is true 
* Added Safety checks to ClearData and other calls of the RaMonoDataHolderBase, to prevent a null reference on Clear during scene destruction

## v1.3.2 - 17/03/2023
* Added OnInitialization method to the RaMonoDataHolderBase 
* Renamed OnDispose to OnDeinitialization

## v1.3.1 - 04/03/2023
* Linked Resolve events to the RaDataHolderBase
* Made the Resolve functionality 
* Made it so the DataHolder channel class' Resolve automatically refers to the SetData Resolve, without requiring an interface cast 

## v1.3.0 - 04/03/2023
* Made Resolve logics the main part of setting data by introducing it as a non-optional parameter
* Made it so the DataHolder class' Resolve automatically refers to the SetData Resolve, without requiring an interface cast

## v1.2.0 - 03/03/2023
* Added Resolve functionality for Set & Clear Data
* Renamed SetData with parameter object to SetRawData to not have the fallback cause runtime issues during a refactor
* Changed ClearData Event send logics

## v1.1.1 - 19/02/2023
* Renamed OnDisplay to OnSetData & OnClear to OnClearData + Corrected Dispose order

## v1.0.0 - 02/02/2023
* Initial Release
