## dev-task02-06a -> dev-task02-07
ファイル入出力の調整
- csv書き込み方法をstreamWriter+OnDestroyの方式に
- 動画は MyFileIO/mp4/じゃなくてStreamingAssets/mp4/で読み込むように
- StreamingAssets/mp4/ に置いた先頭の動画を再生するように
- HMDとキー入力ログはMyLogs/に出力されるように
