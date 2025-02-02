﻿using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Parser;
using HMUI;
using IPA.Utilities;
using System.Reflection;
using UnityEngine;


namespace Nya.UI.ViewControllers
{
    public class NSFWConfirmModalController
    {
        public delegate void ButtonPressed();
        private ButtonPressed yesButtonPressed;
        private ButtonPressed noButtonPressed;

        #region components
        [UIComponent("root")]
        private readonly RectTransform rootTransform;

        [UIComponent("modal")]
        private ModalView modalView;

        [UIComponent("modal")]
        private readonly RectTransform modalTransform;
        #endregion

        [UIAction("yes-click")]
        private void yesNSFW()
        {
            yesButtonPressed?.Invoke();
            parserParams.EmitEvent("close-modal");
        }

        [UIAction("no-click")]
        private void noNSFW()
        {
            noButtonPressed?.Invoke();
            parserParams.EmitEvent("close-modal");
        }

        [UIParams]
        private readonly BSMLParserParams parserParams;

        private void Parse(Transform parentTransform)
        {
            BSMLParser.instance.Parse(BeatSaberMarkupLanguage.Utilities.GetResourceContent(Assembly.GetExecutingAssembly(), "Nya.UI.Views.NSFWConfirmModal.bsml"), parentTransform.gameObject, this);
            FieldAccessor<ModalView, bool>.Set(ref modalView, "_animateParentCanvas", true);
            if (rootTransform != null && modalTransform != null)
            {
                modalTransform.SetParent(rootTransform);
                modalTransform.gameObject.SetActive(false);
            }
        }

        internal void ShowModal(Transform parentTransform, ButtonPressed yesButtonPressedCallback, ButtonPressed noButtonPressedCallback)
        {
            Parse(parentTransform);
            parserParams.EmitEvent("close-modal");
            parserParams.EmitEvent("open-modal");
            yesButtonPressed = yesButtonPressedCallback;
            noButtonPressed = noButtonPressedCallback;
        }

        internal void HideModal()
        {
            if (modalTransform != null)
            {
                modalTransform.GetComponent<ModalView>().Hide(false);
            }
        }
    }
}
