## Fx Wizard / Gesture Wizard

`Fx Wizard` ではFXレイヤーにセットする表情や動的着せ替えアニメーション、 `Gesture Wizard` ではGestureレイヤーにセットするハンドサインアニメーションを設定します。

最低限Fx WizardのHandSign Emoteに表情をセットしたら [次のステップ](7_ActionWizard.md) に進んでください。

### インスペクタ

![6.1.FxWizard.png](img/6.1.FxWizard.png)

#### 全体的な設定

- `Advanced Animations`: オンにすると、左手と右手のジェスチャーに対して別々のアニメーションを設定することができます。
- `HandSign Override` オンにすると、特定のパラメータがセットされている場合は表情固定が有効になります。
- `HandSign Override Parameter`: 表情固定に利用するパラメータ

`HandSign Override Parameter` を有効にした場合は、ParameterWizardにパラメータが登録されていることを確認してください。

#### 個別のアニメーション設定

Emote Wizard のアニメーションは大きく４つのカテゴリに分かれています。
アニメーションの再生条件に適切な設定項目を利用してください。

- `Base Mixin`: 常時再生のアニメーション。他のアニメーションよりも前に適用されます（優先度が低い）
- `HandSign Emote`: ハンドサインに応じて再生されるアニメーション。
- `Parameter Emote`: 単一のパラメータに連動して表示を変化させるアニメーション。
- `Mixin`: 常時再生のアニメーション。他のアニメーションよりも後に適用されます（優先度が高い）
  複数のパラメータに連動して表示を変化させたい場合は、BlendTreeをMixinに組み込んでください。

#### Output zone

- `Generate Animation Controller` Animation Controllerアセットを生成します。
- `Output Asset` 生成されたAnimation Controllerアセットがここにセットされます。
- `Reset Clip` Fx Wizardの場合は、自動生成された表示リセット用のアニメーションがここにセットされます。編集は不要です。

### Base Mixin、Mixin

![6.2.FxWizardMixin.png](img/6.2.FxWizardMixin.png)

一部の項目は初期状態では隠されています。全ての項目を表示するためには、ヘッダーの `Edit Conditions` ならびに `Edit Controls` をオンにしてください。

- `✔︎`: オフにした場合、アニメーションはアバターに含まれません。
- `Name`: Mixinの表示上の名前。この名前はアバターには含まれません。
- `Kind`: Mixinの種類。
- `Conditions`: アニメーションの再生条件。
- `Duration`: アニメーションの切り替わりが完了するまでの時間。
- `Normalized Time`: オンにした場合、パラメータによってアニメーションの再生位置を制御します。
  - `Parameter`: `Normalized Time`のパラメータ
- `Tracking Overrides`: このアニメーションを再生中、まばたきやリップシンク、その他のトラッキングを無効にしてアニメーションの動作を再生します。まばたきを固定する際は `Eyes` 、リップシンクを固定する場合は `Mouth` を設定してください。

### HandSign Emote

![6.3.FxWizardHandSignEmotes.png](img/6.3.FxWizardHandSignEmotes.png)

標準のセットアップで 7 HandSigns を選んだ場合、上記の項目が生成されているはずです。

![6.4.FxWizardHandSignEmoteOpen.png](img/6.4.FxWizardHandSignEmoteOpen.png)

一部の項目は初期状態では隠されています。全ての項目を表示するためには、ヘッダーの `Edit Conditions` 、 `Edit Animations` ならびに `Edit Controls` をオンにしてください。

- `✔︎`: オンにした場合、表情固定用の値を割り当て可能になります。
- `HandSign Override`: 表情固定に利用するパラメータの値
- `Gesture` / `Gesture Other`: ハンドサインの詳細条件
- `Conditions`: アニメーションの再生条件。
- `Clip`: ハンドサインで再生されるアニメーション
- `Clip Left` / `Clip Right`: 左手と右手のハンドサインで再生されるアニメーション
- `Duration`: アニメーションの切り替わりが完了するまでの時間。
- `Normalized Time`: オンにした場合、パラメータによってアニメーションの再生位置を制御します。
  - `Parameter`: `Normalized Time`のパラメータ
- `Tracking Overrides`: このアニメーションを再生中、まばたきやリップシンク、その他のトラッキングを無効にしてアニメーションの動作を再生します。まばたきを固定する際は `Eyes` 、リップシンクを固定する場合は `Mouth` を設定してください。

### Parameter Emote

![6.5.FxWizardParameterEmote.png](img/6.5.FxWizardParameterEmote.png)

一部の項目は初期状態では隠されています。全ての項目を表示するためには、ヘッダーの `Edit Transition Targets` と `Edit Transition Controls` をオンにしてください。（これらの項目は `Emote Kind` が `Transition` の場合のみ表示されます。）

- `✔︎`: オフにした場合、このParameter Emoteはアバターには含まれません。
- `Name`: このParameter Emoteの表示上の名前。この名前はアバターには含まれません。
- `Parameter`: このパラメータに基づいて表示を切り替えます。
- `Emote Kind`: 表示を切り替える方法を選択します。
  - `Unused`: このParameter Emoteを使用しません。
  - `Transition`: ステート遷移を利用して表示を切り替えます。
  - `NormalizedTime`: アニメーションクリップの再生位置をパラメータで制御します。
  - `BlendTree`: 表示を切り替えるためのブレンドツリーを生成します。
- `States`: パラメータの値
- `Targets`: `Transition` を選択した場合、GameObjectの有効・無効を切り替えるアニメーションを自動生成できます。パラメータの値ごとに有効にしたいGameObjectを設定したのち、一番下に表示される `Generate clips from targets` ボタンを押して下さい。

`Collect Parameters` ボタンを押すと、Parameters Wizard上でオンになっている全ての `Parameter Item` と `Default Parameter Item` に対応するParameter Emoteが生成されます。
