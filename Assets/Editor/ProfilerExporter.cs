using UnityEngine;
using UnityEditor;
using UnityEditor.Profiling;
using UnityEditorInternal;
using System.IO;
using System.Text;
using System.Collections.Generic;

public class ProfilerExporter
{
    [MenuItem("Tools/Export Profiler Data to CSV")]
    public static void Export()
    {
        int firstFrame = ProfilerDriver.firstFrameIndex;
        int lastFrame = ProfilerDriver.lastFrameIndex;

        if (firstFrame < 0 || lastFrame < 0)
        {
            Debug.LogError("No profiler data loaded.");
            return;
        }

        // Only export 30 frames from the middle
        int midFrame = (firstFrame + lastFrame) / 2;
        int exportFirst = Mathf.Max(firstFrame, midFrame - 15);
        int exportLast = Mathf.Min(lastFrame, exportFirst + 29);

        Debug.Log($"Exporting frames {exportFirst} to {exportLast}...");

        var sb = new StringBuilder();
        sb.AppendLine("Frame,FrameTime(ms),ThreadName,SampleName,TotalTime(ms),SelfTime(ms),GCAlloc(bytes),CallCount");

        for (int frame = exportFirst; frame <= exportLast; frame++)
        {
            float frameTime = 0f;
            using (var mainView = ProfilerDriver.GetHierarchyFrameDataView(frame, 0,
                HierarchyFrameDataView.ViewModes.MergeSamplesWithTheSameName,
                HierarchyFrameDataView.columnTotalTime, false))
            {
                if (!mainView.valid) continue;
                frameTime = mainView.frameTimeMs;
            }

            // Enumerate all threads by incrementing index until invalid
            for (int threadIdx = 0; ; threadIdx++)
            {
                string threadName = "";
                using (var raw = ProfilerDriver.GetRawFrameDataView(frame, threadIdx))
                {
                    if (!raw.valid) break;
                    threadName = $"{raw.threadName} ({raw.threadGroupName})";
                }

                using (var view = ProfilerDriver.GetHierarchyFrameDataView(frame, threadIdx,
                    HierarchyFrameDataView.ViewModes.MergeSamplesWithTheSameName,
                    HierarchyFrameDataView.columnTotalTime, false))
                {
                    if (!view.valid) continue;
                    ExportSamples(sb, view, view.GetRootItemID(), frame, frameTime, threadName, 0);
                }
            }
        }

        string path = "/Users/haibowang/Downloads/UnityMCPTest/Assets/ProfilerExport.csv";
        File.WriteAllText(path, sb.ToString());
        AssetDatabase.Refresh();
        Debug.Log($"Exported frames {exportFirst}-{exportLast} to {path}");
    }

    static void ExportSamples(StringBuilder sb, HierarchyFrameDataView view, int itemId, int frame, float frameTime, string threadName, int depth)
    {
        if (depth > 5) return; // limit depth to avoid huge files

        var children = new List<int>();
        view.GetItemChildren(itemId, children);

        foreach (var child in children)
        {
            string name = view.GetItemName(child);
            float totalTime = view.GetItemColumnDataAsFloat(child, HierarchyFrameDataView.columnTotalTime);
            float selfTime = view.GetItemColumnDataAsFloat(child, HierarchyFrameDataView.columnSelfTime);
            float gcAlloc = view.GetItemColumnDataAsFloat(child, HierarchyFrameDataView.columnGcMemory);
            float callCount = view.GetItemColumnDataAsFloat(child, HierarchyFrameDataView.columnCalls);

            sb.AppendLine($"{frame},{frameTime:F3},{threadName},{new string(' ', depth * 2)}{name},{totalTime:F3},{selfTime:F3},{gcAlloc},{callCount}");

            ExportSamples(sb, view, child, frame, frameTime, threadName, depth + 1);
        }
    }
}
