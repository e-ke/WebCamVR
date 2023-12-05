## dev-task02-05 -> dev-task02-06a
- SteamVR Plugin import
    - VelocityEstimator.csのみチェックしてimport
- MovMoQシーン
    - main cameraにVelocityEstimator.csをアタッチ
    - それをHMDLogger2で参照、出力
    - productNameをWebCamVRに
    - XRIT
        - XR Interaction Manager, EventSystem：disabled
        - Camera Offset>L/R Controller&~Stabilized：disabled
---
### ビルドしてFocus3で確認231205_1700
- 問題なく起動
- RoundedRectSinglePassシェーダOK
- moQ表示位置OK, キー操作OK
- HMDのログ出力された
### ~~直すとこ~~
~~動画再生されない~~
動画の置き場所間違えただけだった
