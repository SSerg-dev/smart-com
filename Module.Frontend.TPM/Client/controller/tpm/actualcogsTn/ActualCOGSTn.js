﻿Ext.define('App.controller.tpm.actualcogstn.ActualCOGSTn', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'actualcogstn[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'actualcogstn directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'actualcogstn #datatable': {
                    activate: this.onActivateCard
                },
                'actualcogstn #detailform': {
                    activate: this.onActivateCard
                },
                'actualcogstn #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'actualcogstn #detailform #next': {
                    click: this.onNextButtonClick
                },
                'actualcogstn #detail': {
                    click: this.switchToDetailForm
                },
                'actualcogstn #table': {
                    click: this.onTableButtonClick
                },
                'actualcogstn #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'actualcogstn #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'actualcogstn #createbutton': {
                    click: this.onCreateButtonClick
                },
                'actualcogstn #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'actualcogstn #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'actualcogstn #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'actualcogstn #refresh': {
                    click: this.onRefreshButtonClick
                },
                'actualcogstn #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'actualcogstn #exportbutton': {
                    click: this.onExportButtonClick
                },
                'actualcogstn #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'actualcogstn #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'actualcogstn #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                },

                //'actualcogstn #recalculateactualcogstnpreviousyearbutton': {
                //    click: this.onRecalculatePreviousYearButtonClick
                //},

                '#confirmPromoRecalculationCOGSWindow #confirm': {
                    click: this.onConfirmPromoRecalculationButtonClick
                }
            }
        });
    },

    onGridAfterrender: function (grid) {
        //this.callParent(arguments);

        //var resource = 'ActualCOGSTns',
        //    action = 'IsCOGSTnRecalculatePreviousYearButtonAvailable',
        //    allowedActions = [];

        //var currentRoleAPs = App.UserInfo.getCurrentRole().AccessPoints.filter(function (point) {
        //    return point.Resource === resource;
        //});

        //currentRoleAPs.forEach(function (point) {
        //    if (!allowedActions.some(function (action) { return action === point.Action })) {
        //        allowedActions.push(point.Action);
        //    }
        //});

        //if (Ext.Array.contains(allowedActions, action)) {
        //    var store = grid.getStore();
        //    store.on('load', function (store) {
        //        parameters = {};

        //        App.Util.makeRequestWithCallback(resource, action, parameters, function (data) {
        //            if (data) {
        //                var result = Ext.JSON.decode(data.httpResponse.data.value);
        //                if (result.success) {
        //                    var recalculatePreviousYearButton = grid.up('panel').down('#recalculateactualcogstnpreviousyearbutton');
        //                    this.recalculatePreviousYearButton = recalculatePreviousYearButton;
        //                    if (result.isRecalculatePreviousYearButtonAvailable) {
        //                        recalculatePreviousYearButton.setDisabled(false);
        //                    } else {
        //                        recalculatePreviousYearButton.setDisabled(true);
        //                    }
        //                } else {
        //                    App.Notify.pushError(data.message);
        //                }
        //            }
        //        }, function (data) {
        //            App.Notify.pushError(data.message);
        //        });
        //    });
        //}
    },

    onRecalculatePreviousYearButtonClick: function (button) {
        var actualCogsTnPanel = button.up('actualcogstn'),
            parameters = {};

        actualCogsTnPanel.setLoading(true);

        App.Util.makeRequestWithCallback('ActualCOGSTns', 'PreviousYearPromoList', parameters, function (data) {
            if (data) {
                var result = Ext.JSON.decode(data.httpResponse.data.value);
                if (result.success) {
                    var previousYearPromoes = result.promoes;

                    var simplePromoViewerWidget = Ext.widget('basereviewwindow', {
                        title: l10n.ns('core').value('confirmTitle'),
                        itemId: 'confirmPromoRecalculationCOGSWindow',
                        width: '55%',
                        height: '75%',
                        minHeight: 450,
                        minWidth: 600,

                        items: {
                            xtype: 'simplepromoviewer',
                            title: 'Promo for night recalculation'
                        },
                        buttons: [{
                            text: l10n.ns('core', 'buttons').value('confirm'),
                            itemId: 'confirm'
                        }, {
                            text: l10n.ns('core', 'buttons').value('close'),
                            itemId: 'close'
                        }]
                    });

                    var grid = simplePromoViewerWidget.down('simplepromoviewer').down('directorygrid'),
                        store = grid.getStore(),
                        proxy = store.getProxy();

                    var promoArray = new Array();
                    previousYearPromoes.forEach(function (item) {
                        var model = Ext.create('App.model.tpm.promo.SimplePromo');
                        model.data = item;
                        promoArray.push(model);
                    });

                    proxy.data = promoArray;
                    store.load();

                    simplePromoViewerWidget.show();
                    actualCogsTnPanel.setLoading(false);
                } else {
                    actualCogsTnPanel.setLoading(false);
                    App.Notify.pushError(data.message);
                }
            }
        }, function (data) {
            actualCogsTnPanel.setLoading(false);
            App.Notify.pushError(data.message);
        });
    },

    onConfirmPromoRecalculationButtonClick: function (button) {
        var window = button.up('#confirmPromoRecalculationCOGSWindow'),
            parameters = {};

        window.setLoading(true);

        App.Util.makeRequestWithCallback('ActualCOGSTns', 'CreateActualCOGSTnChangeIncidents', parameters, function (data) {
            window.setLoading(false);
            this.recalculatePreviousYearButton.setDisabled(true);
            window.close();
        }, function (data) {
            window.setLoading(false);
            if (data) {
                App.Notify.pushError(data.message);
            }
        })
    },

    onHistoryButtonClick: function (button) {
        var grid = this.getGridByButton(button),
            selModel = grid.getSelectionModel();

        if (selModel.hasSelection()) {
            var panel = grid.up('combineddirectorypanel'),
                model = panel.getBaseModel(),
                viewClassName = App.Util.buildViewClassName(panel, model, 'Historical');

            var baseReviewWindow = Ext.widget('basereviewwindow', { items: Ext.create(viewClassName, { baseModel: model }) });
            baseReviewWindow.show();

            var store = baseReviewWindow.down('grid').getStore();
            var proxy = store.getProxy();
            proxy.extraParams.Id = this.getRecordId(selModel.getSelection()[0]);
        }
    }
});
