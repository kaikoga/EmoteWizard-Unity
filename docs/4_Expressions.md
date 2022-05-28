## Expressions Menuの設定

`Expression Wizard` は登録されている `Expression Item Source` からメニュー項目を収集し、 Expression Menuを出力します。
Emote Wizardでアバターを設定する際は、最初にメニュー項目を作ってから、メニュー項目に対応するパラメータとアニメーションを設定する順番で作業すると効率的です。

Expression Menuを編集する必要がなければ、何もせずに [次のステップ](5_Parameters.md) に進んでください。

### Expression Wizard

![4.1.ExpressionWizard.png](img/4.1.ExpressionWizard.png)

#### 全体的な設定

- `Build as Sub Asset`: オンにした場合、生成されるExpressionMenuアセットが１ファイルにまとまります。

#### Output zone

- `Generate Expression Menu` Expression Menuアセットを生成します。
- `Output Asset` 生成されたExpression Menuアセットがここにセットされます。

### Expression Item Source

![4.2.ExpressionItemSource.png](img/4.2.ExpressionItemSource.png)

アバターに追加するメニュー項目をここに登録します。

#### 全体的な設定

- `Default Prefix`: デフォルトのエモート用メニュー項目を生成する際の配置先を設定します。
- `Populate Default Expression Items`: デフォルトのエモート用メニュー項目を生成します。
- `Group by Folder`: 同じサブフォルダに属するメニュー項目をまとめます。

#### 各アイテム

- `✔︎`: オフにした場合、メニュー項目は生成されません。
- `Icon`: メニュー項目に設定するアイコン。
- `Path`: メニュー項目の配置先。スラッシュ区切りで、サブメニューを指定できます。
- `Parameter`: メニュー項目に対応するパラメータ。（空白で、パラメータ未設定）
- `Value`: メニュー項目を選択した場合にパラメータにセットされる値。
- `ControlType`: メニュー項目のタイプ。選択内容次第で、追加の設定項目が表示されます。
