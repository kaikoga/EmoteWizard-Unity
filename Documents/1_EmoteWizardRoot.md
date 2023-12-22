## Emote Wizardの基本設定

ヒエラルキーを右クリック、またはメニューバーの `GameObject` → `Emote Wizard` で、ヒエラルキーに `Emote Wizard` が追加されます。
Emote Wizardの各種設定やアバターへの反映は全てこのオブジェクトを起点として行います。

細かなカスタマイズが不要な場合は、 `Emote Wizard Root` コンポーネントの `Setup` をクリックして [次のステップ](2_Setup.md) に進んでください。

### Emote Wizard Root

![1.1.EmoteWizardRoot.png](img/1.1.EmoteWizardRoot.png)

#### 一般設定

- `Generated Assets Root`: Emote Wizardが自動生成するアセットの保存先ディレクトリを指定します。
- `Generated Assets Prefix`: Emote Wizardが自動生成するアセットの名前の先頭につける文字列を指定します。
- `Empty Clip`: ここにセットされたアニメーションクリップが各所で「空のアニメーション」として使用されます。必要に応じて自動生成されますので、編集の必要はありません。

#### 各種設定

- `Show Tutorial`: 簡単な説明を日本語で表示します。
- `Copy Paste JSON`: Copy Paste JSON ボタンが表示されます。 Emote Wizard の設定を文字列でコピーペーストできるようになります。
- `List Display Mode`: リスト表示の項目数が多い場合に、表示方法を３タイプから変更することができます。Emote Wizardの全てのコンポーネントに対して有効です。

#### 操作

- `Setup`: Setup Wizard を追加します。
- `Add Empty Data Source`: 空のデータソースを追加します。例えば、特定のアバターのみに実装する着せ替えに関する設定をまとめて有効化・無効化したりできます。
- `Disconnect Output Assets`: 出力アセットの参照を全て切断します。（アセットは削除されずに残りますが、自動生成したアセットは上書きされることがあります。）例えば、Emote Wizardを丸ごとプレハブ化して複数のアバターで使い回す際に押してください。

### Emote Wizard Data Source Factory

![1.2.EmoteWizardDataSourceFactory.png](img/1.2.EmoteWizardDataSourceFactory.png)

`Setup Wizard` で生成したデータソースや `Add Empty Data Source` で追加した空のデータソースにあらかじめ追加されているユーティリティコンポーネントです。

`Setup Wizard` から初期設定を行うことで一通りのデータソースが生成されますが、追加のデータソースを用意してアバターの設定を整理したい場合、ここから生成できます。
