## セットアップ

`Setup Wizard` からEmote Wizardの各コンポーネントを生成できます。
コンポーネントは全て生成することを推奨しますが、例えばGesture Layerを使わない場合にGesture Wizardを使わないなどのカスタマイズは可能です。

`Quick Setup 7 HandSigns` をクリックしてコンポーネントを生成したのち、 `Complete Setup and Remove Me` をクリックしてSetup Wizardを削除して [次のステップ](3_AvatarWizard.md) に進んでください。

### インスペクタ

![2.1.SetupWizard.png](img/2.1.SetupWizard.png)

#### 一般設定

- `Enable Setup Only UI`: オンにした場合、各コンポーネントのSetup only zoneが有効になります。Setup only zone内のボタンはコンポーネントの設定済みデータを全て初期化するため、注意して扱ってください。
- `Generate Wizards`: 全てのコンポーネントを中身が空の状態で生成します。

#### Setup only zone

- `Quick Setup 7 HandSigns`: 全てのコンポーネントを生成して初期化します。
- `Quick Setup 14 HandSigns`: 全てのコンポーネントを生成して初期化します。
- `Complete Setup and Remove Me`: Setup Wizardを削除することができます。（必要になった時点で再度追加できます）

### HandSignsの選び方

以下の方は、 `7 HandSigns` を選んでください。

- 左手と右手のハンドサインに同一の表情を設定する
- 左手と右手のハンドサインに別々の表情を設定する

以下の方は、 `14 HandSigns` を選んでください。

- 左手と右手で同じハンドサインを出した場合に、特別な表情が出るようにする
