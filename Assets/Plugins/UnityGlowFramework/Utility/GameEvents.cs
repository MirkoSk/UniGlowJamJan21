using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniGlow.Utility
{
	public class GameEvents
	{
        public delegate void ScanningHandler(Vector3 trackedPosition, Quaternion trackedRotation);
        public static event ScanningHandler ScanningCompleted;

        public static void CompleteScan(Vector3 trackedPosition, Quaternion trackedRotation)
        {
            ScanningCompleted?.Invoke(trackedPosition, trackedRotation);
        }

        public delegate void ScanStartHandler();
        public static event ScanStartHandler ScanningStarted;

        public static void StartScan()
        {
            ScanningStarted?.Invoke();
        }

        public delegate void POITapHandler(GameObject poiTapped);
        public static event POITapHandler POITapped;

        public static void TapPOI(GameObject poiTapped)
        {
            POITapped?.Invoke(poiTapped);
        }

        public delegate void TimeChangeHandler(int newTimeDomain);
        public static event TimeChangeHandler TimeDomainChanged;

        public static void ChangeTimeDomain(int newTimeDomain)
        {
            TimeDomainChanged?.Invoke(newTimeDomain);
        }
    }
}