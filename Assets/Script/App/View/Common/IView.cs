using System;
namespace App.View.Common
{
    /// <summary>
    /// 画面部品要素が、イベントの通知を受け取るインタフェース
    /// </summary>
    public interface IView
    {

        /// <summary>
        /// 描画開始(初描画or再描画)の通知を、上位要素(PanelController等）から受け取る
        /// </summary>
        void OnNotify();

        /// <summary>
        /// 画面部品要素が、再描画準備完了したらtrue, 未完了中はfalseを返す。
        /// 非同期に(ネットワーク越し等で)描画処理が遅れているのを判定するため。
        /// </summary>
        bool IsReady();

    }
}
