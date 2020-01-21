Ext.define('App.controller.tpm.promo.PromoChangeStatus', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                // Кнопки изменения состояния промо
                'promoeditorcustom #btn_publish': {
                    click: this.onPublishButtonClick
                },
                'promoeditorcustom #btn_undoPublish': {
                    click: this.onUndoPublishButtonClick
                },
                'promoeditorcustom #btn_sendForApproval': {
                    click: this.onSendForApprovalButtonClick
                },
                'promoeditorcustom #btn_approve': {
                    click: this.onApproveButtonClick
                },
                'promoeditorcustom #btn_plan': {
                    click: this.onPlanButtonClick
                },
                'promoeditorcustom #btn_close': {
                    click: this.onToClosePromoButtonClick
                },
                'promoeditorcustom #btn_backToFinished': {
                    click: this.onBackToFinishedPromoButtonClick
                },
                'promoeditorcustom #btn_cancel': {
                    click: this.onCancelButtonClick
                },
                'promoeditorcustom #btn_reject': {
                    click: this.onRejectButtonClick
                },
                'promoeditorcustom #btn_backToDraftPublished': {
                    click: this.onBackToDraftPublishedButtonClick
                },

                'rejectreasonselectwindow #apply': {
                    click: this.onApplyActionButtonClick
                },

                'promoeditorcustom #btn_changeStatus': {
                    click: this.onPromoChangeStatusBtnClick
                },
                'promochangestatuswindow > directorygrid': {
                    selectionchange: this.onSelectPromoStatusGridSelectionChange
                },
                'promochangestatuswindow #select': {
                    click: this.onApplySelectedPromoStatusClick
                }
            }
        });
    },

    onPublishButtonClick: function (button) {
        var window = button.up('promoeditorcustom');
        var promoController = App.app.getController('tpm.promo.Promo');
        var checkValid = promoController.validatePromoModel(window);
        if (checkValid === '') {
            var record = promoController.getRecord(window);

            window.previousStatusId = window.statusId;
            window.statusId = button.statusId;
            window.promoName = promoController.getPromoName(window);

            var model = promoController.buildPromoModel(window, record);
            promoController.saveModel(model, window, false, true);
            promoController.updateStatusHistoryState();
        } else {
            App.Notify.pushInfo(checkValid);
        }
    },

    onUndoPublishButtonClick: function (button) {
        var window = button.up('promoeditorcustom');
        var promoController = App.app.getController('tpm.promo.Promo');
        var checkValid = promoController.validatePromoModel(window);
        if (checkValid === '') {
            var record = promoController.getRecord(window);

            window.down('#PromoUpliftLockedUpdateCheckbox').setValue(false);
            window.down('[name = PlanPromoUpliftPercent]').setValue(null);

            window.previousStatusId = window.statusId;
            window.statusId = button.statusId;
            window.promoName = 'Unpublish Promo';
            window.down('#btn_recalculatePromo').hide();

            var model = promoController.buildPromoModel(window, record);
            promoController.saveModel(model, window, false, true);

            // если во время возврата была открыта вкладка Calculations/Activity нужно переключиться с них
            var btn_promo = button.up('window').down('container[name=promo]');
            var btn_work_flow = button.up('window').down('#btn_changes');

            if (!btn_promo.hasCls('selected') && !btn_work_flow.hasCls('selected')) {// && !btn_support.hasCls('selected')) {
                promoController.onPromoButtonClick(btn_promo);
            }
        } else {
            App.Notify.pushInfo(checkValid);
        }
    },

    onSendForApprovalButtonClick: function (button) {
        var window = button.up('promoeditorcustom');
        var promoController = App.app.getController('tpm.promo.Promo');
        var checkValid = promoController.validatePromoModel(window);

        var isStep7Complete = !window.down('#btn_promoBudgets_step1').hasCls('notcompleted');
        var isStep8Complete = !window.down('#btn_promoBudgets_step2').hasCls('notcompleted');
        var isStep9Complete = !window.down('#btn_promoBudgets_step3').hasCls('notcompleted');

        if (checkValid === '' && isStep7Complete && isStep8Complete && isStep9Complete) {
            var record = promoController.getRecord(window);

            window.previousStatusId = window.statusId;
            window.statusId = button.statusId;
            window.promoName = promoController.getPromoName(window);

            var model = promoController.buildPromoModel(window, record);
            promoController.saveModel(model, window, false, true);
        } else {
            App.Notify.pushInfo(checkValid);
        }
    },

    onApproveButtonClick: function (button) {
        var window = button.up('promoeditorcustom');
        var promoController = App.app.getController('tpm.promo.Promo');
        var checkValid = promoController.validatePromoModel(window);

        // TODO: необходимо точно выяснить ограничения
        var isStep7Complete = !window.down('#btn_promoBudgets_step1').hasCls('notcompleted');
        var isStep8Complete = !window.down('#btn_promoBudgets_step2').hasCls('notcompleted');
        var isStep9Complete = !window.down('#btn_promoBudgets_step3').hasCls('notcompleted');

        if (checkValid === '' && (isStep7Complete && isStep8Complete && isStep9Complete)) {
            // окно подтверждения
            Ext.Msg.show({
                title: l10n.ns('tpm', 'text').value('Confirmation'),
                msg: l10n.ns('tpm', 'Promo').value('Confirm Approval'),
                fn: function (btn) {
                    if (btn === 'yes') {
                        // Логика для согласования
                        var record = promoController.getRecord(window);

                        window.previousStatusId = window.statusId;
                        window.statusId = button.statusId;
                        window.promoName = promoController.getPromoName(window);

                        var model = promoController.buildPromoModel(window, record);

                        // если есть доступ, то через сохранение (нужно протестировать механизмы)
                        var pointsAccess = App.UserInfo.getCurrentRole().AccessPoints;
                        var access = pointsAccess.find(function (element) {
                            return (element.Resource == 'Promoes' && element.Action == 'Patch') ||
                                (element.Resource == 'PromoGridViews' && element.Action == 'Patch');
                        });

                        if (access) {
                            promoController.saveModel(model, window, false, true);
                        }
                        else {
                            promoController.changeStatusPromo(record.data.Id, button.statusId, window);
                        }
                    }
                },
                scope: promoController,
                icon: Ext.Msg.QUESTION,
                buttons: Ext.Msg.YESNO,
                buttonText: {
                    yes: l10n.ns('tpm', 'button').value('confirm'),
                    no: l10n.ns('tpm', 'button').value('cancel')
                }
            });
        } else {
            App.Notify.pushInfo(checkValid);
        }
    },

    onPlanButtonClick: function (button) {
        var window = button.up('promoeditorcustom');
        var promoController = App.app.getController('tpm.promo.Promo');
        var checkValid = promoController.validatePromoModel(window);

        // TODO: необходимо точно выяснить ограничения
        var isStep7Complete = !window.down('#btn_promoBudgets_step1').hasCls('notcompleted');
        var isStep8Complete = !window.down('#btn_promoBudgets_step2').hasCls('notcompleted');
        var isStep9Complete = !window.down('#btn_promoBudgets_step3').hasCls('notcompleted');

        if (checkValid === '' && (isStep7Complete && isStep8Complete && isStep9Complete)) {
            var record = promoController.getRecord(window);

            window.previousStatusId = window.statusId;
            window.statusId = button.statusId;
            window.promoName = promoController.getPromoName(window);

            var model = promoController.buildPromoModel(window, record);

            // если есть доступ, то через сохранение (нужно протестировать механизмы)
            var pointsAccess = App.UserInfo.getCurrentRole().AccessPoints;
            var access = pointsAccess.find(function (element) {
                return element.Resource == 'Promoes' && element.Action == 'Patch';
            });

            if (access) {
                promoController.saveModel(model, window, false, true);
            }
            else {
                promoController.changeStatusPromo(record.data.Id, button.statusId, window);
            }
        } else {
            App.Notify.pushInfo(checkValid);
        }
    },

    onToClosePromoButtonClick: function (button) {
        var window = button.up('promoeditorcustom');
        var promoController = App.app.getController('tpm.promo.Promo');

        // TODO: необходимо точно выяснить ограничения
        var isStep7Complete = !window.down('#btn_promoBudgets_step1').hasCls('notcompleted');
        var isStep8Complete = !window.down('#btn_promoBudgets_step2').hasCls('notcompleted');
        var isStep9Complete = !window.down('#btn_promoBudgets_step3').hasCls('notcompleted');

        var CheckValid = promoController.validatePromoModel(window);

        //Упрощенная проверка для закрытия
        var promomechanic = window.down('promomechanic');
        var v1 = promomechanic.down('numberfield[name=MarsMechanicDiscount]').validate();
        var v2 = promomechanic.down('numberfield[name=PlanInstoreMechanicDiscount]').validate();
        var v3 = promomechanic.down('textarea[name=PromoComment]').validate();
        var isPromoValid = v1 && v2 && v3;

        //TODO: переделать
        var promoactivity = window.down('promoactivity');
        var actM = promoactivity.down('searchcombobox[name=ActualInstoreMechanicId]');
        var actMIsValid = !(actM.rawValue === "");
        var actAISP = promoactivity.down('numberfield[name=ActualInStoreShelfPrice]');
        var actAISPIsValid = !(actAISP.value === null);
        var isActivityPromoValid = actMIsValid && actAISPIsValid;
        if (CheckValid === '' && !isActivityPromoValid) {
            CheckValid = 'In order to close promo Actual In Store in Activity must be filled.';
        }

        if ((CheckValid === '') && (isStep7Complete && isStep8Complete && isStep9Complete) && (isPromoValid && isActivityPromoValid)) {
            var record = promoController.getRecord(window);

            window.previousStatusId = window.statusId;
            window.statusId = button.statusId;
            window.promoName = promoController.getPromoName(window);

            var model = promoController.buildPromoModel(window, record);

            // если есть доступ, то через сохранение (нужно протестировать механизмы)
            var pointsAccess = App.UserInfo.getCurrentRole().AccessPoints;
            var access = pointsAccess.find(function (element) {
                return element.Resource == 'Promoes' && element.Action == 'Patch';
            });

            if (access) {
                promoController.saveModel(model, window, false, true);
            }
            else {
                promoController.changeStatusPromo(record.data.Id, button.statusId, window);
            }
        } else {
            App.Notify.pushInfo(CheckValid);
        }
    },

    onBackToFinishedPromoButtonClick: function (button) {
        var window = button.up('promoeditorcustom');
        var promoController = App.app.getController('tpm.promo.Promo');
        var checkValid = promoController.validatePromoModel(window);

        // TODO: необходимо точно выяснить ограничения
        var isStep7Complete = !window.down('#btn_promoBudgets_step1').hasCls('notcompleted');
        var isStep8Complete = !window.down('#btn_promoBudgets_step2').hasCls('notcompleted');
        var isStep9Complete = !window.down('#btn_promoBudgets_step3').hasCls('notcompleted');

        //Упрощенная проверка для закрытия
        var promomechanic = window.down('promomechanic');
        var v1 = promomechanic.down('numberfield[name=MarsMechanicDiscount]').validate();
        var v2 = promomechanic.down('numberfield[name=PlanInstoreMechanicDiscount]').validate();
        var v3 = promomechanic.down('textarea[name=PromoComment]').validate();
        var isPromoValid = v1 && v2 && v3;

        if ((checkValid === '') && (isStep7Complete && isStep8Complete && isStep9Complete)) {
            if (isPromoValid) {
                var record = promoController.getRecord(window);

                window.previousStatusId = window.statusId;
                window.statusId = button.statusId;
                window.promoName = promoController.getPromoName(window);

                var model = promoController.buildPromoModel(window, record);

                // если есть доступ, то через сохранение (нужно протестировать механизмы)
                var pointsAccess = App.UserInfo.getCurrentRole().AccessPoints;
                var access = pointsAccess.find(function (element) {
                    return element.Resource == 'Promoes' && element.Action == 'Patch';
                });

                promoController.changeStatusPromo(record.data.Id, button.statusId, window);
            } else {
                return;
            }
        } else {
            App.Notify.pushInfo(checkValid);
        }
    },

    onCancelButtonClick: function (button) {
        var window = button.up('promoeditorcustom');
        var promoController = App.app.getController('tpm.promo.Promo');
        var record = promoController.getRecord(window);
        window.previousStatusId = window.statusId;
        window.statusId = button.statusId;
        window.promoName = promoController.getPromoName(window);
        var model = promoController.buildPromoModel(window, record);
        window.readOnly = true;
        promoController.saveModel(model, window, false, true);
        window.down('#btn_showlog').hide();
        window.down('#btn_recalculatePromo').hide();
        window.down('#changePromo').hide();
        window.down('#cancelPromo').hide();
        window.down('#closePromo').show();
    },

    onRejectButtonClick: function (button) {
        var window = button.up('window');
        var promoController = App.app.getController('tpm.promo.Promo');
        var record = promoController.getRecord(window);
        this.showCommentWindow(record, window);
    },

    showCommentWindow: function (record, window) {
        var rejectreasonselectwindow = Ext.widget('rejectreasonselectwindow');
        rejectreasonselectwindow.record = record;
        rejectreasonselectwindow.promowindow = window;
        rejectreasonselectwindow.show();
    },

    //Подтверждение отклонения промо
    onApplyActionButtonClick: function (button) {
        var windowReject = button.up('rejectreasonselectwindow');
        var promoController = App.app.getController('tpm.promo.Promo');
        var commentField = windowReject.down('textarea[name=comment]');
        var rejectReasonField = windowReject.down('searchfield[name=RejectReasonId]');

        // проверка на валидность формы
        if (!rejectReasonField.isValid() || (commentField.isVisible() && !commentField.isValid())) {
            rejectReasonField.validate();
            commentField.validate();
            return;
        }

        parameters = {
            rejectPromoId: breeze.DataType.Guid.fmtOData(windowReject.record.data.Id),
            rejectReasonId: breeze.DataType.Guid.fmtOData(rejectReasonField.getValue()),
            rejectComment: breeze.DataType.String.fmtOData(commentField.getValue())
        };

        windowReject.setLoading(l10n.ns('core').value('savingText'));

        App.Util.makeRequestWithCallback('Promoes', 'DeclinePromo', parameters, function (data) {
            var result = Ext.JSON.decode(data.httpResponse.data.value);

            if (result.success) {
                // TODO: логика при успешном отклонении
                var windowPromo = Ext.ComponentQuery.query('promoeditorcustom')[0];
                windowPromo.setLoading(l10n.ns('core').value('savingText'));

                App.model.tpm.promo.Promo.load(windowReject.record.data.Id, {
                    callback: function (newModel, operation) {
                        if (newModel) {
                            var grid = Ext.ComponentQuery.query('#promoGrid')[0];
                            var directorygrid = grid ? grid.down('directorygrid') : null;

                            windowPromo.promoId = newModel.data.Id;
                            windowPromo.model = newModel;
                            promoController.reFillPromoForm(windowPromo, newModel, directorygrid);
                        }
                        else {
                            windowPromo.setLoading(false);
                        }
                    }
                });

                windowReject.close();
            } else {
                App.Notify.pushError(l10n.ns('tpm', 'text').value('failedLoadData'));
            }

            windowReject.setLoading(false);
        }, function (data) {
            if (data.body != undefined) {
                if (data.body['odata.error'] != undefined) {
                    App.Notify.pushError(data.body['odata.error'].innererror.message);
                } else {
                    App.Notify.pushError(data.message);
                }
            } else {
                App.Notify.pushError(data.message);
            }
            windowReject.setLoading(false);
        });
    },

    onBackToDraftPublishedButtonClick: function (button) {
        var window = button.up('promoeditorcustom');
        var promoController = App.app.getController('tpm.promo.Promo');
        var record = promoController.getRecord(window);

        window.previousStatusId = window.statusId;
        window.statusId = button.statusId;
        window.promoName = promoController.getPromoName(window);

        var model = promoController.buildPromoModel(window, record);

        var pointsAccess = App.UserInfo.getCurrentRole().AccessPoints;
        var access = pointsAccess.find(function (element) {
            return element.Resource == 'Promoes' && element.Action == 'Patch';
        });

        promoController.changeStatusPromo(record.data.Id, button.statusId, window);
        promoController.updateStatusHistoryState();
    },

    onPromoChangeStatusBtnClick: function (button) {
        Ext.widget('promochangestatuswindow').show();
    },

    onSelectPromoStatusGridSelectionChange: function (el, selected) {
        var applyButton = el.view.up('promochangestatuswindow').down('#select');
        if (applyButton) {
            if (applyButton.disabled && selected.length > 0) {
                applyButton.setDisabled(false);
            } else if (!applyButton.disabled && selected.length == 0) {
                applyButton.setDisabled(true);
            }
        }
    },

    onApplySelectedPromoStatusClick: function (button) {
        var promoController = App.app.getController('tpm.promo.Promo'),
            promowindow = Ext.ComponentQuery.query('promoeditorcustom')[0],
            window = button.up('promochangestatuswindow'),
            grid = window.down('directorygrid'),
            selModel = grid.getSelectionModel(),
            record = promoController.getRecord(promowindow);

        if (selModel.hasSelection()) {
            var selected = selModel.getSelection()[0];

            if (record && record.data && selected && selected.data && record.data.PromoStatusId !== selected.data.Id) {
                Ext.Msg.show({
                    title: l10n.ns('tpm', 'text').value('Confirmation'),
                    msg: l10n.ns('tpm', 'Promo').value('ConfirmPromoStatusChange'),
                    fn: function (btn) {
                        if (btn === 'yes') {
                            window.close();

                            promowindow.previousStatusId = promowindow.statusId;
                            promowindow.statusId = selected.data.Id;
                            promowindow.promoName = promoController.getPromoName(promowindow);

                            var model = promoController.buildPromoModel(promowindow, record);
                            promoController.saveModel(model, promowindow, false, true);
                        }
                    },
                    scope: this,
                    icon: Ext.Msg.QUESTION,
                    buttons: Ext.Msg.YESNO,
                    buttonText: {
                        yes: l10n.ns('tpm', 'button').value('confirm'),
                        no: l10n.ns('tpm', 'button').value('cancel')
                    }
                });
            } else if (record && record.data && selected && selected.data && record.data.PromoStatusId === selected.data.Id) {
                App.Notify.pushInfo(l10n.ns('tpm', 'Promo').value('FailToSelectStatus'));
            }
        }
    }
});
