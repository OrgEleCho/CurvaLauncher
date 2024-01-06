using System;
using System.Collections.Generic;

namespace CurvaLauncher.Utilities;

/// <summary>
/// 排序工具
/// </summary>
static class SortingUtils
{
    /// <summary>
    /// 冒泡排序
    /// </summary>
    /// <typeparam name="T">元素类型</typeparam>
    /// <typeparam name="TKey">排序键类型</typeparam>
    /// <param name="list">列表</param>
    /// <param name="keySelector">键选择器</param>
    /// <param name="reverse">是否倒序</param>
    public static void BubbleSortBy<T, TKey>(IList<T> list, Func<T, TKey> keySelector, bool reverse) where TKey : IComparable<TKey>
    {
        int end = list.Count - 1;
        for (int i = 0; i < end; i++)
            for (int j = 0; j < end - i; j++)
                if (keySelector(list[j]).CompareTo(keySelector(list[j + 1])) > 0 ^ reverse)
                    (list[j], list[j + 1]) = (list[j + 1], list[j]);
    }

    /// <summary>
    /// 快速排序
    /// </summary>
    /// <typeparam name="T">元素类型</typeparam>
    /// <typeparam name="TKey">排序键类型</typeparam>
    /// <param name="list">列表</param>
    /// <param name="keySelector">键选择器</param>
    /// <param name="reverse">是否倒序</param>
    public static void QuickSortBy<T, TKey>(IList<T> list, Func<T, TKey> keySelector, bool reverse) where TKey : IComparable<TKey>
    {
        void QuickSortCoreBy(int start, int end)
        {
            if (start >= end)
                return;

            int pivot = start;
            for (int i = start + 1; i <= end; i++)
            {
                if (keySelector(list[start]).CompareTo(keySelector(list[i])) > 0 ^ reverse)
                {
                    pivot++;
                    (list[pivot], list[i]) =
                        (list[i], list[pivot]);
                }
            }

            if (pivot != start)
                (list[pivot], list[start]) =
                    (list[start], list[pivot]);


            QuickSortCoreBy(start, pivot - 1);
            QuickSortCoreBy(pivot + 1, end);
        }

        QuickSortCoreBy(0, list.Count - 1);
    }

    /// <summary>
    /// 选择排序
    /// </summary>
    /// <typeparam name="T">元素类型</typeparam>
    /// <typeparam name="TKey">排序键类型</typeparam>
    /// <param name="list">列表</param>
    /// <param name="keySelector">键选择器</param>
    /// <param name="reverse">是否倒序</param>
    public static void SelectionSortBy<T, TKey>(IList<T> list, Func<T, TKey> keySelector, bool reverse) where TKey : IComparable<TKey>
    {
        for (int i = 0; i < list.Count; i++)
        {
            T selected = list[i];
            TKey selectedKey = keySelector(selected);
            int selectedIndex = i;

            for (int j = i + 1; j < list.Count; j++)
            {
                T current = list[j];
                TKey currentKey = keySelector(current);
                if (currentKey.CompareTo(selectedKey) < 0 ^ reverse)
                {
                    selected = current;
                    selectedKey = currentKey;
                    selectedIndex = j;
                }
            }

            (list[i], list[selectedIndex]) = (list[selectedIndex], list[i]);
        }
    }

    /// <summary>
    /// 插入排序
    /// </summary>
    /// <typeparam name="T">元素类型</typeparam>
    /// <typeparam name="TKey">排序键类型</typeparam>
    /// <param name="list">列表</param>
    /// <param name="keySelector">键选择器</param>
    /// <param name="reverse">是否倒序</param>
    public static void InsertionSortBy<T, TKey>(IList<T> list, Func<T, TKey> keySelector, bool reverse) where TKey : IComparable<TKey>
    {
        for (int i = 1; i < list.Count; i++)
        {
            T current = list[i];
            TKey currentKey = keySelector(current);

            int j = i - 1;
            while (j >= 0 && keySelector(list[j]).CompareTo(currentKey) > 0 ^ reverse)
            {
                list[j + 1] = list[j];
                j--;
            }

            list[j + 1] = current;
        }
    }

    /// <summary>
    /// 希尔排序
    /// </summary>
    /// <typeparam name="T">元素类型</typeparam>
    /// <typeparam name="TKey">排序键类型</typeparam>
    /// <param name="list">列表</param>
    /// <param name="keySelector">键选择器</param>
    /// <param name="reverse">是否倒序</param>
    public static void ShellSortBy<T, TKey>(IList<T> list, Func<T, TKey> keySelector, bool reverse) where TKey : IComparable<TKey>
    {
        for (int gap = list.Count / 2; gap >= 1; gap /= 2)
        {
            for (int i = gap; i < list.Count; i++)
            {
                T current = list[i];
                TKey currentKey = keySelector(current);

                int j = i - gap;
                while (j >= 0 && keySelector(list[j]).CompareTo(currentKey) > 0 ^ reverse)
                {
                    list[j + gap] = list[j];
                    j -= gap;
                }

                list[j + gap] = current;
            }
        }
    }



    /// <summary>
    /// 冒泡排序 (升序)
    /// </summary>
    /// <typeparam name="T">元素类型</typeparam>
    /// <typeparam name="TKey">排序键类型</typeparam>
    /// <param name="list">列表</param>
    /// <param name="keySelector">键选择器</param>
    public static void BubbleSortBy<T, TKey>(IList<T> list, Func<T, TKey> keySelector) where TKey : IComparable<TKey>
        => BubbleSortBy(list, keySelector, false);

    /// <summary>
    /// 冒泡排序 (升序)
    /// </summary>
    /// <typeparam name="T">元素类型</typeparam>
    /// <param name="list">列表</param>
    public static void BubbleSort<T>(IList<T> list) where T : IComparable<T>
        => BubbleSortBy(list, item => item, false);

    /// <summary>
    /// 冒泡排序
    /// </summary>
    /// <typeparam name="T">元素</typeparam>
    /// <param name="list">列表</param>
    /// <param name="reverse">是否倒序</param>
    public static void BubbleSort<T>(IList<T> list, bool reverse) where T : IComparable<T>
        => BubbleSortBy(list, item => item, reverse);

    /// <summary>
    /// 快速排序 (升序)
    /// </summary>
    /// <typeparam name="T">元素类型</typeparam>
    /// <typeparam name="TKey">排序键类型</typeparam>
    /// <param name="list">列表</param>
    /// <param name="keySelector">键选择器</param>
    public static void QuickSortBy<T, TKey>(IList<T> list, Func<T, TKey> keySelector) where TKey : IComparable<TKey>
        => QuickSortBy(list, keySelector, false);

    /// <summary>
    /// 快速排序 (升序)
    /// </summary>
    /// <typeparam name="T">元素类型</typeparam>
    /// <param name="list">列表</param>
    public static void QuickSort<T>(IList<T> list) where T : IComparable<T>
        => QuickSortBy(list, item => item, false);

    /// <summary>
    /// 快速排序
    /// </summary>
    /// <typeparam name="T">元素</typeparam>
    /// <param name="list">列表</param>
    /// <param name="reverse">是否倒序</param>
    public static void QuickSort<T>(IList<T> list, bool reverse) where T : IComparable<T>
        => QuickSortBy(list, item => item, reverse);

    /// <summary>
    /// 选择排序 (升序)
    /// </summary>
    /// <typeparam name="T">元素类型</typeparam>
    /// <typeparam name="TKey">排序键类型</typeparam>
    /// <param name="list">列表</param>
    /// <param name="keySelector">键选择器</param>
    public static void SelectionSortBy<T, TKey>(IList<T> list, Func<T, TKey> keySelector) where TKey : IComparable<TKey>
        => SelectionSortBy(list, keySelector, false);

    /// <summary>
    /// 选择排序 (升序)
    /// </summary>
    /// <typeparam name="T">元素类型</typeparam>
    /// <param name="list">列表</param>
    public static void SelectionSort<T>(IList<T> list) where T : IComparable<T>
        => SelectionSortBy(list, item => item, false);

    /// <summary>
    /// 选择排序
    /// </summary>
    /// <typeparam name="T">元素</typeparam>
    /// <param name="list">列表</param>
    /// <param name="reverse">是否倒序</param>
    public static void SelectionSort<T>(IList<T> list, bool reverse) where T : IComparable<T>
        => SelectionSortBy(list, item => item, reverse);

    /// <summary>
    /// 插入排序 (升序)
    /// </summary>
    /// <typeparam name="T">元素类型</typeparam>
    /// <typeparam name="TKey">排序键类型</typeparam>
    /// <param name="list">列表</param>
    /// <param name="keySelector">键选择器</param>
    public static void InsertionSortBy<T, TKey>(IList<T> list, Func<T, TKey> keySelector) where TKey : IComparable<TKey>
        => InsertionSortBy(list, keySelector, false);

    /// <summary>
    /// 插入排序 (升序)
    /// </summary>
    /// <typeparam name="T">元素类型</typeparam>
    /// <param name="list">列表</param>
    public static void InsertionSort<T>(IList<T> list) where T : IComparable<T>
        => InsertionSortBy(list, item => item, false);

    /// <summary>
    /// 插入排序
    /// </summary>
    /// <typeparam name="T">元素</typeparam>
    /// <param name="list">列表</param>
    /// <param name="reverse">是否倒序</param>
    public static void InsertionSort<T>(IList<T> list, bool reverse) where T : IComparable<T>
        => InsertionSortBy(list, item => item, reverse);

    /// <summary>
    /// 希尔排序 (升序)
    /// </summary>
    /// <typeparam name="T">元素类型</typeparam>
    /// <typeparam name="TKey">排序键类型</typeparam>
    /// <param name="list">列表</param>
    /// <param name="keySelector">键选择器</param>
    public static void ShellSortBy<T, TKey>(IList<T> list, Func<T, TKey> keySelector) where TKey : IComparable<TKey>
        => ShellSortBy(list, keySelector, false);

    /// <summary>
    /// 希尔排序 (升序)
    /// </summary>
    /// <typeparam name="T">元素类型</typeparam>
    /// <param name="list">列表</param>
    public static void ShellSort<T>(IList<T> list) where T : IComparable<T>
        => ShellSortBy(list, item => item, false);

    /// <summary>
    /// 希尔排序
    /// </summary>
    /// <typeparam name="T">元素</typeparam>
    /// <param name="list">列表</param>
    /// <param name="reverse">是否倒序</param>
    public static void ShellSort<T>(IList<T> list, bool reverse) where T : IComparable<T>
        => ShellSortBy(list, item => item, reverse);
}