## Action Wizard

`Action Wizard` ではActionレイヤーにセットするエモートとAFKアニメーションを設定します。

凝った改変をしない場合は編集不要です。必要がなければ何もせずに [次のステップ](8_Publish.md)に進んでください。

### インスペクタ

![7.1.ActionWizard.png](img/7.1.ActionWizard.png)

#### 全体的な設定

- `Fixed Transition Duration`: オンにしておいてください。
- `Action Select Parameter` このパラメータが0以外の値になるとエモートが再生されます。
- `Action Emotes` エモートの設定
- `AFK Select Enabled` オンにすると、AFKアニメーションを選択可能になります。
- `AFK Select Parameter` AFKアニメーションを選択するためのパラメータ
- `AFK Emotes`: AFKアニメーションの設定
- `Default AFK Emotes`: デフォルトのAFKアニメーションの設定

`Action Select Parameter` や `AFK Select Parameter` を有効にした場合は、ParameterWizardにパラメータが登録されていることを確認してください。

#### Output zone

- `Generate Animation Controller` Animation Controllerアセットを生成します。
- `Output Asset` 生成されたAnimation Controllerアセットがここにセットされます。

### Action Emote / AFK Emote

![7.2.ActionWizardActionEmotes.png](img/7.2.ActionWizardActionEmotes.png)

標準のセットアップを行った場合、上記の項目が生成されているはずです。

![7.3.ActionWizardActionEmoteOpen.png](img/7.3.ActionWizardActionEmoteOpen.png)

一部の項目は初期状態では隠されています。全ての項目を表示するためには、ヘッダーの `Edit Layer Blend` と `Edit Transition` をオンにしてください。

説明は省略します。生成されたAnimator Controllerを見て雰囲気を掴んでください。

