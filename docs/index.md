# Emote Wizard 1.0.0 ドキュメント

## 目次

- [index.md](index.md) このドキュメント
- [Emote Wizardの基本設定](1_EmoteWizard.md) ・・・Emote Wizardを使う上でのUIの設定など
- [セットアップ](2_SetupWizard.md) ・・・環境構築
- [Avatar Wizard](3_AvatarWizard.md) ・・・セットアップ対象のアバターの基本設定
- [Expression Wizard](4_ExpressionWizard.md) ・・・アクションメニュー項目の設定
- [Parameters Wizard](5_ParametersWizard.md) ・・・アニメーションパラメータの設定
- [Fx Wizard / Gesture Wizard](6_FxWizard.md) ・・・表情と動的着せ替えとハンドサインの設定
- [Action Wizard](7_ActionWizard.md) ・・・エモートとAFKアニメーションの設定
- [アバターのビルド](8_Publish.md) ・・・Emote Wizardから行える設定を完了したらすること

## これは何？

VRChatのAvatar SDK3.0 用のアバターのセットアップを支援するツールです。
アップロードしているアバターが多い人向け、かつUnityの基本操作とAvatar 3.0の基本がわかってる人向けとなっております。

Emote Wizardは、アバターと同じシーン上に配置することで、アバターの表情や動的な着せ替えアニメーションとExpression Menu・Expression Parameterを一括管理するツールです。

- AvatarDescriptorのベース設定（管轄外！）
- 衣装着せ替え・アクセサリ着脱・テクスチャ編集（管轄外！）
- **表情編集・動的な着せ替えアニメーション（Emote Wizardはここを担当）**

Emote Wizardは、Avatar SDK 3.0用のVRCAvatarDescriptorのうち、以下の設定を占有します。

- Playable Layers
  - Gesture Layer
  - Action Layer
  - FX Layer
  - Sitting Layer
- Expressions
  - Expression Menu
  - Expression Parameter

これ以外の設定はEmote Wizardからは利用することができませんので、必要に応じてUnityやVRCSDK標準の機能でセットアップを行うか、他のツールと併用してください。

### 動作環境

Emote Wizard 1.0.0 は以下の環境で動作を確認しています。

- Windows 10
- Unity2019.4.29f1
- VRCSDK3-AVATAR-2021.08.04.11.23_Public

これ以外の環境でも動作するかもしれませんが、自己責任でお願いします。
また、上記の環境を満たしている場合も、動作保証や他のツールとの相性などのサポート対応は致しかねる場合があります。ご了承ください。

### 利用条件

本ソフトウェアは MIT License にて配布いたします。私的利用を超えた範囲で利用または再配布を行う場合は、 MIT License の条件に従ってください。
正式なライセンス文書は [LICENSE](../LICENSE) を参照してください。

また、kaikogaは、Emote Wizardの正常動作に関して一切の保障をいたしません。このドキュメントではUnityの基本操作も特に説明はいたしません。

### 内部仕様

以下の仕様が干渉する場合は、他のツールと併用することができません。

**Emote WizardはAnimator Controllerを自動生成します。** 独自のAnimator Controllerを提供するアバターや、Animator Controllerのステートマシンを編集して機能を追加するタイプのツールとは相性が良くないと思います。

**Emote WizardはExpression Menu、Expression Parameterを自動生成します。** 全てのアニメーションパラメータはEmote Wizardで管理する必要があります。他のアセットで利用するパラメータはParameter Wizardなどから設定することもできますが、上級者向けとなります。

**Expression Menuの項目名をスラッシュ区切りで入力することによって、自動的にサブフォルダが作成されます。** 

**アニメーションのWrite Defaultsはオフとなります。** Emote Wizardは、Write Defaultsがオフの状態で再生終了したアニメーションのシェイプキーが元の値に戻るように、リセット用のアニメーションクリップを自動生成します。これにはAvatar Wizardにセットしたアバターの現在の値が使われます。

例１：
- シーン上でシェイプキーの値が0
- 表情アニメーションによってシェイプキーの値を50に変更
- 表情アニメーション終了時、シェイプキーの値は0に戻る

例２：
- シーン上で服を着ている（GameObjectがアクティブ）
- 着せ替えトグルアニメーションによって服を脱ぐ（GameObjectがアクティブではなくなる）
- 着せ替えトグルアニメーション終了時、GameObjectはアクティブに戻る

**アニメーションのVRCAnimatorTrackingControlはEmote Wizardが管理します。** いわゆるまばたき固定・リップシンク固定に相当する機能です。Tracking Overrideが有効なアニメーションを再生中は `Animation` にセットされ、再生完了すると `Tracking` に戻るようにアニメーションが構築されます。これは完璧ではないので、複数のレイヤーで同じ部位に対してTracking Overrideを設定した場合、固定が意図せず解除される場合があります。 **特に、Action Layerにセットされるエモートは `Eyes` と `Mouth` 以外の全てのトラッキングを固定し、再生完了後に解除します。** このため、Tracking Overrideについて `Eyes` と `Mouth` 以外の設定は可能ですが、推奨されません。

