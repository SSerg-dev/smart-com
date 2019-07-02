Ext.define('App.controller.tpm.promo.Promo', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'promo directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onPromoGridSelectionChange,
                    afterrender: this.onGridPromoAfterrender,
                    extfilterchange: this.onExtFilterChange,
                    load: this.onPromoGridStoreLoad
                },
                'promo': {
                    beforedestroy: this.onPromoGridBeforeDestroy
                },
                'promo #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'promo #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'promo #createbutton': {
                    click: this.onCreateButtonClick
                },
                'promo #createinoutbutton': {
                    click: this.onCreateInOutButtonClick
                },
                'promo #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'promo #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'promo #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'promo #refresh': {
                    click: this.onRefreshButtonClick
                },
                'promo #canchangestateonlybutton': {
                    click: this.onShowEditableButtonClick
                },

                // promo window
                'promoeditorcustom': {
                    afterrender: this.onPromoEditorCustomAfterrender,
                    beforerender: this.onPromoEditorCustomBeforerender,
                    show: this.onPromoEditorCustomShow,
                    close: this.onPromoEditorCustomClose,
                },

                // promo main toolbar btn
                'promoeditorcustom #btn_promo': {
                    click: this.onPromoButtonClick
                },
                'promoeditorcustom #btn_support': {
                    click: this.onSupportButtonClick
                },
                'promoeditorcustom #btn_promoBudgets': {
                    click: this.onPromoBudgetsButtonClick
                },
                'promoeditorcustom #btn_promoActivity': {
                    click: this.onPromoActivityButtonClick
                },
                'promoeditorcustom #btn_changes': {
                    click: this.onChangesButtonClick
                },
                'promoeditorcustom #btn_summary': {
                    click: this.onSummaryButtonClick
                },

                // promo basic steps
                'promoeditorcustom #btn_promo_step1': {
                    click: this.onPromoButtonStep1Click
                },
                'promoeditorcustom #btn_promo_step2': {
                    click: this.onPromoButtonStep2Click
                },
                'promoeditorcustom #btn_promo_step3': {
                    click: this.onPromoButtonStep3Click
                },
                'promoeditorcustom #btn_promo_step4': {
                    click: this.onPromoButtonStep4Click
                },
                'promoeditorcustom #btn_promo_step5': {
                    click: this.onPromoButtonStep5Click
                },
                'promoeditorcustom #btn_promo_step6': {
                    click: this.onPromoButtonStep6Click
                },

                // support steps
                'promoeditorcustom #btn_support_step1': {
                    click: this.onSupportButtonStep1Click
                },
                'promoeditorcustom #btn_support_step2': {
                    click: this.onSupportButtonStep2Click
                },
                'promoeditorcustom #btn_support_step3': {
                    click: this.onSupportButtonStep3Click
                },

                // promo budgets steps
                'promoeditorcustom #btn_promoBudgets_step1': {
                    click: this.onPromoBudgetsButtonStep1Click
                },
                'promoeditorcustom #btn_promoBudgets_step2': {
                    click: this.onPromoBudgetsButtonStep2Click
                },
                'promoeditorcustom #btn_promoBudgets_step3': {
                    click: this.onPromoBudgetsButtonStep3Click
                },

                // promo activity steps
                'promoeditorcustom #btn_promoActivity_step1': {
                    click: this.onPromoActivityButtonStep1Click
                },
                'promoeditorcustom #btn_promoActivity_step2': {
                    click: this.onPromoActivityButtonStep2Click
                },
                'promoeditorcustom #savePromo': {
                    click: this.onSavePromoButtonClick
                },
                'promoeditorcustom #saveAndClosePromo': {
                    click: this.onSaveAndClosePromoButtonClick
                },
                'promoeditorcustom #closePromo': {
                    click: this.onClosePromoButtonClick
                },
                'promoeditorcustom #cancelPromo': {
                    click: this.onCancelPromoButtonClick
                },
                'promoeditorcustom #changePromo': {
                    click: this.onChangePromoButtonClick
                },

                // CustomTopToolbar
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
                'promoeditorcustom #btn_cancel': {
                    click: this.onCancelButtonClick
                },
                'promoeditorcustom #btn_close': {
                    click: this.onToClosePromoButtonClick
                },
                'promoeditorcustom #btn_backToFinished': {
                    click: this.onBackToFinishedPromoButtonClick
                },
                'promoeditorcustom #btn_reject': {
                    click: this.onRejectButtonClick
                },
                'promoeditorcustom #btn_history': {
                    click: this.onPromoHistoryButtonClick
                },
                'promoeditorcustom #btn_recalculatePromo': {
                    click: this.recalculatePromo
                },
                'rejectreasonselectwindow #apply': {
                    click: this.onApplyActionButtonClick
                },

                // import/export
                'promo #exportbutton': {
                    click: this.onExportButtonClick
                },
                'promo #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'promo #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'promo #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                },

                // promo budgets
                'promobudgets': {
                    afterrender: this.onPromoBudgetsAfterRender
                },
                'promobudgets #promoBudgets_step1 numberfield': {
                    change: this.onPromoBudgetsStep1Change
                },

                // promo calculation                
                'promomechanic numberfield[name=MarsMechanicDiscount]': {
                    change: this.onMarsMechanicDiscountChange
                },
                'promocalculation #calculations_step2 numberfield[name=PlanPromoIncrementalLSV]': {
                    change: this.onPlanPromoIncrementalLSVChange
                },
                'promocalculation #calculations_step2 numberfield': {
                    change: this.onCalculationPlanChange
                },
                'promocalculation #calculations_step3 numberfield': {
                    change: this.onCalculationPlanChange
                },

                // promo activity
                'promoactivity': {
                    afterrender: this.onPromoActivityAfterRender
                },

                'promoactivity #promoActivity_step2 #activityUploadPromoProducts': {
                    click: this.onActivityUploadPromoProductsClick
                },

                // методы решают проблему с "Настроить таблицу"
                'promo customheadermenu #gridsettings': {
                    click: this.onGridSettingsClick
                },
                'gridsettingswindow #save': {
                    click: this.onGridSettingsSaveClick
                },
                'gridsettingswindow': {
                    close: this.onGridSettingsWindowClose
                },

                'promoeditorcustom #btn_showlog': {
                    click: this.onShowLogButtonClick
                },
                // Promo Budgets Details Window
                'promobudgetsdetailswindow': {
                    afterrender: this.onPromoBudgetsDetailsWindowAfterRender
                },
                // Promo Finance Details Window
                'promofinancedetailswindow': {
                    afterrender: this.onPromoFinanceDetailsWindowAfterRender
                },
                // Promo Activity Details Window
                'promoactivitydetailswindow': {
                    afterrender: this.onPromoActivityDetailsWindowAfterRender
                },
                // Promo Product Subrange Details Window
                'promoproductsubrangedetailswindow': {
                    afterrender: this.onPromoProductSubrangeDetailsWindowAfterRender
                },
                'promoeditorcustom #activityDetailButton': {
                    click: this.onSummaryDetailButtonClick
                },
                'promoeditorcustom #budgetDetailButton': {
                    click: this.onSummaryDetailButtonClick
                },
                'promoeditorcustom #financeDetailButton': {
                    click: this.onSummaryDetailButtonClick
                },
                'promoeditorcustom #brandTechButton': {
                    click: this.onSummaryDetailButtonClick
                },
                'promoeditorcustom #basicPromoPanel': {
                    afterrender: this.onBasicPanelAfterrender
                },

                // choose client
                'promoclient #promoClientSettignsBtn': {
                    click: this.onPromoClientSettignsClick
                },
                'promoclient #choosePromoClientBtn': {
                    click: this.onChoosePromoClientClick
                },
                'promoclientchoosewindow clienttree basetreegrid': {
                    checkchange: this.onClientTreeCheckChange
                },
                'promoclientchoosewindow #choose': {
                    click: this.onChooseClientTreeClick
                },

                // choose product
                'promobasicproducts #choosePromoProductsBtn': {
                    click: this.onСhoosePromoProductsBtnClick
                },
                'promobasicproducts #promoBasicProducts_FilteredList': {
                    click: this.onFilteredListBtnClick
                },
                'promobasicproducts #promoBasicProducts_ProductList': {
                    click: this.onProductListBtnClick
                },
                'promoproductchoosewindow producttree basetreegrid': {
                    checkchange: this.onProductTreeCheckChange
                },
                'promoproductchoosewindow #choose': {
                    click: this.onChooseProductTreeClick
                },
            }
        });
    },

    onGridPromoAfterrender: function (grid) {
        var store = grid.getStore();

        store.on('load', function (store) {
            var selectionModel = grid.getSelectionModel();
            if (selectionModel.hasSelection()) {
                var view = grid.getView();

                if (view && view.positionY) {
                    var jspData = $(view.getEl().dom).data('jsp');

                    jspData.scrollToY(view.positionY, false);
                    view.positionY = 0;
                }
            }
        });

        this.onGridAfterrender(grid);
    },

    onBasicPanelAfterrender: function (component, eOpts) {
        $(component.getTargetEl().dom).on('jsp-scroll-y', function (event, scrollPositionY, isAtTop, isAtBottom) {
            var panel = component.up();
            if (panel) { // TODO: отрефакторить?
                var btnStep1 = panel.down('#btn_promo_step1');
                var btnStep2 = panel.down('#btn_promo_step2');
                var btnStep3 = panel.down('#btn_promo_step3');
                var btnStep4 = panel.down('#btn_promo_step4');
                var btnStep5 = panel.down('#btn_promo_step5');
                var btnStep6 = panel.down('#btn_promo_step6');

                var formStep1 = panel.down('#promo_step1');
                var formStep2 = panel.down('#promo_step2');
                var formStep3 = panel.down('#promo_step3');
                var formStep4 = panel.down('#promo_step4');
                var formStep5 = panel.down('#promo_step5');
                var formStep6 = panel.down('#promo_step6');

                if (formStep6.needToSetHeight && isAtTop) {
                    formStep6.setHeight(panel.getHeight() - 20);
                    formStep6.needToSetHeight = false;
                }

                var h1 = formStep1.height;
                var h1_2 = h1 + formStep2.height;
                var h1_2_3 = h1_2 + formStep3.height;
                var h1_2_3_4 = h1_2_3 + formStep4.height;
                var h1_2_3_4_5 = h1_2_3_4 + formStep5.height;
                var h1_2_3_4_5_6 = h1_2_3_4_5 + formStep6.height;

                var _deltaY = scrollPositionY + 100;

                // Step 1 - client
                if (_deltaY <= h1) {
                    btnStep1.addClass('selected');
                    btnStep2.removeCls('selected');
                    btnStep3.removeCls('selected');
                    btnStep4.removeCls('selected');
                    btnStep5.removeCls('selected');
                    btnStep6.removeCls('selected');

                    formStep1.header.addClass('promo-header-item-active');
                    formStep2.header.removeCls('promo-header-item-active');
                    formStep3.header.removeCls('promo-header-item-active');
                    formStep4.header.removeCls('promo-header-item-active');
                    formStep5.header.removeCls('promo-header-item-active');
                    formStep6.header.removeCls('promo-header-item-active');

                    // Step 2 - product
                } else if (_deltaY > h1 && _deltaY <= h1_2) {
                    btnStep1.removeCls('selected');
                    btnStep2.addClass('selected');
                    btnStep3.removeCls('selected');
                    btnStep4.removeCls('selected');
                    btnStep5.removeCls('selected');
                    btnStep6.removeCls('selected');

                    formStep1.header.removeCls('promo-header-item-active');
                    formStep2.header.addClass('promo-header-item-active');
                    formStep3.header.removeCls('promo-header-item-active');
                    formStep4.header.removeCls('promo-header-item-active');
                    formStep5.header.removeCls('promo-header-item-active');
                    formStep6.header.removeCls('promo-header-item-active');

                    var mousedownEvent = document.createEvent('MouseEvents');
                    mousedownEvent.initEvent('mousedown', true, true)
                    panel.el.dom.dispatchEvent(mousedownEvent);

                    // Step 3 - mechanic
                } else if (_deltaY > h1_2 && _deltaY <= h1_2_3) {
                    event.preventDefault();

                    btnStep1.removeCls('selected');
                    btnStep2.removeCls('selected');
                    btnStep3.addClass('selected');
                    btnStep4.removeCls('selected');
                    btnStep5.removeCls('selected');
                    btnStep6.removeCls('selected');

                    formStep1.header.removeCls('promo-header-item-active');
                    formStep2.header.removeCls('promo-header-item-active');
                    formStep3.header.addClass('promo-header-item-active');
                    formStep4.header.removeCls('promo-header-item-active');
                    formStep5.header.removeCls('promo-header-item-active');
                    formStep6.header.removeCls('promo-header-item-active');

                    var mousedownEvent = document.createEvent('MouseEvents');
                    mousedownEvent.initEvent('mousedown', true, true)
                    panel.el.dom.dispatchEvent(mousedownEvent);

                    // Step 4 - period
                } else if (_deltaY > h1_2_3 && _deltaY <= h1_2_3_4) {
                    btnStep1.removeCls('selected');
                    btnStep2.removeCls('selected');
                    btnStep3.removeCls('selected');
                    btnStep4.addClass('selected');
                    btnStep5.removeCls('selected');
                    btnStep6.removeCls('selected');

                    formStep1.header.removeCls('promo-header-item-active');
                    formStep2.header.removeCls('promo-header-item-active');
                    formStep3.header.removeCls('promo-header-item-active');
                    formStep4.header.addClass('promo-header-item-active');
                    formStep5.header.removeCls('promo-header-item-active');
                    formStep6.header.removeCls('promo-header-item-active');


                    var mousedownEvent = document.createEvent('MouseEvents');
                    mousedownEvent.initEvent('mousedown', true, true)
                    panel.el.dom.dispatchEvent(mousedownEvent);

                    // Step 5 - event
                } else if (_deltaY > h1_2_3_4 && _deltaY <= h1_2_3_4_5) {
                    btnStep1.removeCls('selected');
                    btnStep2.removeCls('selected');
                    btnStep3.removeCls('selected');
                    btnStep4.removeCls('selected');
                    btnStep5.addClass('selected');
                    btnStep6.removeCls('selected');

                    formStep1.header.removeCls('promo-header-item-active');
                    formStep2.header.removeCls('promo-header-item-active');
                    formStep3.header.removeCls('promo-header-item-active');
                    formStep4.header.removeCls('promo-header-item-active');
                    formStep5.header.addClass('promo-header-item-active');
                    formStep6.header.removeCls('promo-header-item-active');

                    var mousedownEvent = document.createEvent('MouseEvents');
                    mousedownEvent.initEvent('mousedown', true, true)
                    panel.el.dom.dispatchEvent(mousedownEvent);

                    // Step 6 - settings
                } else if (_deltaY > h1_2_3_4_5 && _deltaY <= h1_2_3_4_5_6) {
                    btnStep1.removeCls('selected');
                    btnStep2.removeCls('selected');
                    btnStep3.removeCls('selected');
                    btnStep4.removeCls('selected');
                    btnStep5.removeCls('selected');
                    btnStep6.addClass('selected');

                    formStep1.header.removeCls('promo-header-item-active');
                    formStep2.header.removeCls('promo-header-item-active');
                    formStep3.header.removeCls('promo-header-item-active');
                    formStep4.header.removeCls('promo-header-item-active');
                    formStep5.header.removeCls('promo-header-item-active');
                    formStep6.header.addClass('promo-header-item-active');

                    var mousedownEvent = document.createEvent('MouseEvents');
                    mousedownEvent.initEvent('mousedown', true, true)
                    panel.el.dom.dispatchEvent(mousedownEvent);
                } else {
                    console.log('undefined step');
                }
            }
        });

        var mainTab = component.up('promoeditorcustom').down('button[itemId=btn_promo]');
        var stepButtons = component.up('panel[name=promo]').down('custompromotoolbar');
        checkMainTab(stepButtons, mainTab);
    },


    // открытие окон просмотра детальной информации из дашборда
    onSummaryDetailButtonClick: function (button) {
        Ext.widget(button.windowName).show();
    },

    onPromoEditorCustomAfterrender: function () {
        this.hideEditButtonForSomeRole();
    },

    onPromoEditorCustomShow: function () {
        /*
        var promoBudgetsDetailsWindow = Ext.widget('promobudgetsdetailswindow');
        var promoActivityDetailsWindow = Ext.widget('promoactivitydetailswindow');
        var promoProductSubrangeDetailsWindow = Ext.widget('promoproductsubrangedetailswindow');
        */
    },

    onPromoEditorCustomBeforerender: function () {
        this.setCustomTips();
    },

    setCustomTips: function () {
        var elementsWithTips = Ext.ComponentQuery.query('*[customTip*=]');

        elementsWithTips.forEach(function (el) {
            var me = el;

            // Перезапись стандартного создания ToolTip
            Ext.override(me, {
                afterFirstLayout: function () {
                    var me = this;
                    this.callParent(arguments);

                    me.fieldLabelTip = Ext.create('Ext.tip.ToolTip', {
                        target: me.labelEl,
                        preventLoseFocus: true,
                        trackMouse: true,
                        html: me.customTip
                    });
                }
            });
        })
    },

    onPromoEditorCustomClose: function () {
        var toolbar = Ext.ComponentQuery.query('readonlydirectorytoolbar');
        if (toolbar.length > 0)
            toolbar[0].setDisabled(false);

        // закрываем подключение к хабу
        $.connection.logHub.server.disconnectFromHub();
    },

    // =============== Steps ===============

    // promo main toolbar btn
    onPromoButtonClick: function (button) {
        this.setButtonsState(button);
    },

    onSupportButtonClick: function (button) {
        this.setButtonsState(button);
    },

    onPromoBudgetsButtonClick: function (button) {
        this.setButtonsState(button);
    },

    onPromoActivityButtonClick: function (button) {
        this.setButtonsState(button);
    },
    onSummaryButtonClick: function (button) {
        var me = this,
            promoeditorcustom = button.up('promoeditorcustom'),
            mask = promoeditorcustom.setLoading(true);
        // для того чтобы маска отрисовалась первой
        Ext.Function.defer(me.fillSummaryPanel, 1, me, [button, mask]);
    },
    fillSummaryPanel: function (button, mask) {
        var me = this,
            window = button.up('window'),
            record = me.getRecord(window);

        me.updateSummaryInformationPanel(window, record);
        var summary = window.down('panel[name=summary]');

        me.updateSummaryPlanFactLabels(summary, record);

        me.setButtonsState(button);

        var planActivityContainer = summary.down('container[name=planActivityContainer]'),
            factActivityContainer = summary.down('container[name=factActivityContainer]'),
            budgetsFieldset = summary.down('panel[name=budgetsFieldset]'),
            financeFieldset = summary.down('fieldset[name=financeFieldset]');
        // fact activity Data
        var factInc = record.get('ActualPromoIncrementalLSV'),
            factBL = record.get('ActualPromoBaselineLSV'),
            factChartData;
        if (!factInc && !factBL) {
            // Если нет данных неободимо чтобы график был серым
            factChartData = [
                { name: 'Baseline', value: 0 },
                { name: 'Incremental LSV', value: 0 },
                { name: 'No Data', value: 1 }
            ]
        } else {
            factChartData = [
                { name: 'Baseline', value: factBL },
                { name: 'Incremental LSV', value: factInc },
            ]
        }
        // plan activity Data
        var planInc = record.get('PlanPromoIncrementalLSV'),
            planBL = record.get('PlanPromoBaselineLSV'),
            planChartData;
        if (!planInc && !planBL) {
            // Если нет данных неободимо чтобы график был серым
            planChartData = [
                { name: 'Baseline', value: 0 },
                { name: 'Incremental LSV', value: 0 },
                { name: 'No Data', value: 1 }
            ]
        } else {
            planChartData = [
                { name: 'Baseline', value: planBL },
                { name: 'Incremental LSV', value: planInc },
            ]
        }
        // finance Data
        var planNSV = record.get('PlanPromoNetNSV') || 0,
            factNSV = record.get('ActualPromoNetNSV') || 0,
            planIncNSV = record.get('PlanPromoNetIncrementalNSV') || 0,
            factIncNSV = record.get('ActualPromoNetIncrementalNSV') || 0,
            planIncEarn = record.get('PlanPromoNetIncrementalEarning]') || 0,
            factIncEarn = record.get('ActualPromoNetIncrementalEarnings') || 0;

        var financeChartData = [
            { name: 'Promo Net NSV', planValue: planNSV, factValue: factNSV },
            { name: 'Net Incremental NSV', planValue: planIncNSV, factValue: factIncNSV },
            { name: 'Net Incremental Earnings', planValue: planIncEarn, factValue: factIncEarn },
        ];
        // budget Data
        var planShopper = record.get('PlanPromoTIShopper') || 0,
            factShopper = record.get('ActualPromoTIShopper') || 0,
            planMarketing = record.get('PlanPromoTIMarketing') || 0,
            factMarketing = record.get('ActualPromoTIMarketing') || 0,
            planCost = record.get('PlanPromoCostProduction') || 0,
            factCost = record.get('ActualPromoCostProduction') || 0,
            planBranding = record.get('PlanPromoBranding') || 0,
            factBranding = record.get('ActualPromoBranding') || 0,
            planBTL = record.get('PlanPromoBTL') || 0,
            factBTL = record.get('ActualPromoBTL') || 0;

        budgetData = [
            { type: 'Plan', shopper: planShopper, marketing: planMarketing, cost: planCost, brand: planBranding, btl: planBTL },
            { type: 'Actual', shopper: factShopper, marketing: factMarketing, cost: factCost, brand: factBranding, btl: factBTL }
        ]
        // Если графики уже добавлены в дашборд, просто перезагружаем их сторы
        if (factActivityContainer.items.length != 0) {
            var planActChartStore = planActivityContainer.down('promoactivitychart').getStore(),
                factActChartStore = factActivityContainer.down('promoactivitychart').getStore(),
                budgetsChartStore = budgetsFieldset.down('promobudgetschart').getStore(),
                financeChartStore = financeFieldset.down('promofinancechart').getStore();
            planActChartStore.loadData(planChartData);
            factActChartStore.loadData(factChartData);
            budgetsChartStore.loadData(budgetData);
            financeChartStore.loadData(financeChartData);
        } else {
            // Добавляем графики в дашборд, если их ещё нет там
            factActivityContainer.add({
                xtype: 'promoactivitychart',
                store: Ext.create('Ext.data.Store', {
                    storeId: 'factpromoactivitystore',
                    fields: ['name', 'value'],
                    data: factChartData
                }),
            });

            planActivityContainer.add({
                xtype: 'promoactivitychart',
                store: Ext.create('Ext.data.Store', {
                    storeId: 'planpromoactivitystore',
                    fields: ['name', 'value'],
                    data: planChartData
                }),
            });

            budgetsFieldset.add({
                xtype: 'promobudgetschart',
                store: Ext.create('Ext.data.Store', {
                    storeId: 'promobudgetsstore',
                    fields: ['type', 'shopper', 'marketing', 'cost', 'brand', 'btl'],
                    data: budgetData
                }),
            });

            financeFieldset.add([{
                xtype: 'promofinancechart',
                store: Ext.create('Ext.data.Store', {
                    storeId: 'promofinancechartstore',
                    fields: ['name', 'planValue', 'factValue'],
                    data: financeChartData
                })
            }]);
        }
        mask.destroy();
    },

    // Обновление данных на первой панели дашборда
    updateSummaryInformationPanel: function (window, record) {
        var promoInformationPanel = window.down('panel[name=promoInformationPanel]'),
            promoNameLabel = promoInformationPanel.down('label[name=promoNameLabel]'),
            instoreMechanicLabel = promoInformationPanel.down('label[name=instoreMechanicLabel]'),
            marsMechanicLabel = promoInformationPanel.down('label[name=marsMechanicLabel]'),
            durationDatesLabel = promoInformationPanel.down('label[name=durationDatesLabel]'),
            hierarchyLabel = promoInformationPanel.down('label[name=hierarchyLabel]'),
            brandTechButton = promoInformationPanel.down('button[name=brandTechButton]'),
            dispatchesDatesLabel = promoInformationPanel.down('label[name=dispatchesDatesLabel]'),
            summaryStatusField = promoInformationPanel.down('fieldset[name=summaryStatusField]'),
            promoEventNameButton = promoInformationPanel.down('button[name=promoEventNameButton]');

        var promoName = Ext.String.format('{0} (ID: {1})', record.get('Name'), record.get('Number'));
        promoNameLabel.setText(promoName);
        instoreMechanicLabel.setText(record.get('MechanicIA'));
        marsMechanicLabel.setText(record.get('Mechanic'));

        var durationText = Ext.String.format('{0} - {1}', Ext.Date.format(record.get('StartDate'), 'd.m.Y'), Ext.Date.format(record.get('EndDate'), 'd.m.Y'));
        durationDatesLabel.setText(durationText);
        var clientHierarchy = record.get('ClientHierarchy');
        hierarchyLabel.update({ formattedHierarchy: renderWithDelimiter(clientHierarchy, ' > ', '  ') })
        var btName = record.get('BrandTechName');
        if (btName == '') {
            brandTechButton.setText();
            brandTechButton.addCls('promo-brandtech-button-empty');

        } else {
            brandTechButton.setText(btName);
            brandTechButton.removeCls('promo-brandtech-button-empty');
        }

        var dispatchDatesText = Ext.String.format('{0} - {1}', Ext.Date.format(record.get('DispatchesStart'), 'd.m.Y'), Ext.Date.format(record.get('DispatchesEnd'), 'd.m.Y'));
        dispatchesDatesLabel.setText(dispatchDatesText);
        summaryStatusField.update({ StatusColor: record.get('PromoStatusColor'), StatusName: record.get('PromoStatusName') });
        promoEventNameButton.setText(record.get('PromoEventName'));
    },

    // Обновление текстовых данных план/факт 
    updateSummaryPlanFactLabels: function (summary, record) {
        var me = this;
        var planROI = record.get('PlanPromoNetROIPercent') || 0,
            factROI = record.get('ActualPromoNetROIPercent') || 0,
            template = '<span class="promo-value-{0}">{1}%</span>{2}';
        planROI = Ext.util.Format.round(planROI, 2);
        factROI = Ext.util.Format.round(factROI, 2);
        var planROIStyled = Ext.String.format(template, 'default', planROI, '');
        var factROIStyled = me.getStyledFactValue(planROI, factROI, template);

        var planROIlabel = summary.down('label[name=planroilabel]'),
            factROIlabel = summary.down('label[name=factroilabel]');

        planROIlabel.update({ value: planROIStyled });
        factROIlabel.update({ value: factROIStyled });


        var planUplift = record.get('PlanPromoUpliftPercent') || 0,
            factUplift = record.get('ActualPromoUpliftPercent') || 0;
        planUplift = Ext.util.Format.round(planUplift, 2);
        factUplift = Ext.util.Format.round(factUplift, 2);

        var planUpliftStyled = Ext.String.format(template, 'default', planUplift, '');
        var factUpliftStyled = me.getStyledFactValue(planUplift, factUplift, template);

        var planUpliftlabel = summary.down('label[name=planupliftlabel]'),
            factUpliftlabel = summary.down('label[name=factupliftlabel]');

        planUpliftlabel.update({ value: planUpliftStyled });
        factUpliftlabel.update({ value: factUpliftStyled });

        var planNetUplift = record.get('PlanPromoNetUpliftPercent') || 0,
            factNetUplift = record.get('ActualPromoNetUpliftPercent') || 0;
        planNetUplift = Ext.util.Format.round(planNetUplift, 2);
        factNetUplift = Ext.util.Format.round(factNetUplift, 2);

        var planNetUpliftStyled = Ext.String.format(template, 'default', planNetUplift, '');
        var factNetUpliftStyled = me.getStyledFactValue(planNetUplift, factNetUplift, template);

        var planNetUpliftlabel = summary.down('label[name=plannetupliftlabel]'),
            factNetUpliftlabel = summary.down('label[name=factnetupliftlabel]');

        planNetUpliftlabel.update({ value: planNetUpliftStyled });
        factNetUpliftlabel.update({ value: factNetUpliftStyled });
    },

    // Получение Цифры с иконкой для фактического значения в сравнении с плановым
    getStyledFactValue: function (planValue, factValue, template) {
        var isNegative = planValue > factValue;
        var isPositive = planValue < factValue;
        var style = 'default',
            icon = '';
        if (isNegative) {
            style = 'negative';
            icon = '<span class="mdi mdi-menu-down negative-result-glyph" style="font-family:MaterialDesignIcons;"></span>'
        } else if (isPositive) {
            style = 'positive';
            icon = '<span class="mdi mdi-menu-up positive-result-glyph" style="font-family:MaterialDesignIcons;"></span>';
        }
        return Ext.String.format(template, style, factValue, icon);
    },

    onChangesButtonClick: function (button) {
        this.setButtonsState(button);
        var window = button.up('window');
        var approvalhistory = window.down('container[name=changes]');
        var parentHeight = approvalhistory.up().getHeight();
        approvalhistory.down('fieldset').setHeight(parentHeight - 13);

        if (!approvalhistory.isLoaded) {
            var promoeditorcustom = button.up('promoeditorcustom');
            var promoId = promoeditorcustom.promoId;

            if (promoId != null) {
                promoeditorcustom.setLoading(true);

                var parameters = {
                    promoKey: promoId
                };
                App.Util.makeRequestWithCallback('PromoStatusChanges', 'PromoStatusChangesByPromo', parameters, function (data) {
                    var result = Ext.JSON.decode(data.httpResponse.data.value);
                    if (result.success) {
                        var tpl = Ext.create('App.view.tpm.common.approvalHistoryTpl').formatTpl;
                        var tplDecline = Ext.create('App.view.tpm.common.approvalHistoryDeclineTpl').formatTpl;
                        var itemsArray = [];
                        for (var i = 0; i < result.data.length; i++) {
                            if (i == result.data.length - 1) {
                                result.data[i].IsLast = true;
                            } else {
                                result.data[i].IsLast = false;
                            }

                            // если есть Comment, то значит промо было оклонено
                            if (result.data[i].RejectReasonId !== null) {
                                itemsArray.push({
                                    html: tplDecline.apply(result.data[i]),
                                });
                            }
                            else {
                                itemsArray.push({
                                    html: tpl.apply(result.data[i]),
                                });
                            }
                        }

                        var approvalhistoryPanel = approvalhistory.down('fieldset > panel');
                        approvalhistoryPanel.removeAll();
                        approvalhistoryPanel.add(itemsArray);
                        approvalhistoryPanel.doLayout();

                        approvalhistory.isLoaded = false;
                        approvalhistory.historyArray = result.data;
                        promoeditorcustom.setLoading(false);
                    } else {
                        promoeditorcustom.setLoading(false);
                        App.Notify.pushError(l10n.ns('tpm', 'text').value('failedLoadData'));
                    }
                });
            }
        }
    },
    // переключение между вкладками формы промо
    setButtonsState: function (button) {
        var window = button.up('window'),
            itemNamesArray = ['promo', 'promoBudgets', 'promoActivity', 'summary', 'changes'],
            buttonItem = button.itemId.replace('btn_', '');
        itemNamesArray.forEach(function (item) {
            var selectedButton = item == buttonItem,
                query = Ext.String.format('[name={0}]', item),
                curWin = window.down(Ext.String.format('container{0}', query)) || window.down(Ext.String.format('panel{0}', query));
            curWin.setVisible(selectedButton);
            var curBtn = window.down(Ext.String.format('#btn_{0}', item));
            if (selectedButton) {
                curBtn.addClass('selected');
            } else {
                curBtn.removeCls('selected');
            }
        })
    },

    // promo steps
    onPromoButtonStep1Click: function (button) {
        var container = button.up('window').down('container[name=promo]');

        container.down('#btn_promo_step2').removeCls('selected');
        container.down('#btn_promo_step3').removeCls('selected');
        container.down('#btn_promo_step4').removeCls('selected');
        container.down('#btn_promo_step5').removeCls('selected');
        container.down('#btn_promo_step6').removeCls('selected');

        var jspData = $(container.down('panel[name=basicPromo]').getTargetEl().dom).data('jsp');
        jspData.scrollToY(0, true);
        button.addClass('selected');
    },

    onPromoButtonStep2Click: function (button) {
        var container = button.up('window').down('container[name=promo]');

        container.down('#btn_promo_step1').removeCls('selected');
        container.down('#btn_promo_step3').removeCls('selected');
        container.down('#btn_promo_step4').removeCls('selected');
        container.down('#btn_promo_step5').removeCls('selected');
        container.down('#btn_promo_step6').removeCls('selected');

        var jspData = $(container.down('panel[name=basicPromo]').getTargetEl().dom).data('jsp');
        var el = $(container.down('promobasicproducts#promo_step2').getTargetEl().dom);
        jspData.scrollToElement(el, true, true);
        button.addClass('selected');
    },

    onPromoButtonStep3Click: function (button) {
        var container = button.up('window').down('container[name=promo]');

        container.down('#btn_promo_step1').removeCls('selected');
        container.down('#btn_promo_step2').removeCls('selected');
        container.down('#btn_promo_step4').removeCls('selected');
        container.down('#btn_promo_step5').removeCls('selected');
        container.down('#btn_promo_step6').removeCls('selected');

        var jspData = $(container.down('panel[name=basicPromo]').getTargetEl().dom).data('jsp');
        var el = $(container.down('promomechanic[itemId=promo_step3]').getTargetEl().dom);
        jspData.scrollToElement(el, true, true);
        button.addClass('selected');
    },

    onPromoButtonStep4Click: function (button) {
        var container = button.up('window').down('container[name=promo]');

        container.down('#btn_promo_step1').removeCls('selected');
        container.down('#btn_promo_step2').removeCls('selected');
        container.down('#btn_promo_step3').removeCls('selected');
        container.down('#btn_promo_step5').removeCls('selected');
        container.down('#btn_promo_step6').removeCls('selected');

        var jspData = $(container.down('panel[name=basicPromo]').getTargetEl().dom).data('jsp');
        var el = $(container.down('promoperiod[itemId=promo_step4]').getTargetEl().dom);
        jspData.scrollToElement(el, true, true);
        button.addClass('selected');
    },

    onPromoButtonStep5Click: function (button) {
        var container = button.up('window').down('container[name=promo]');

        container.down('#btn_promo_step1').removeCls('selected');
        container.down('#btn_promo_step2').removeCls('selected');
        container.down('#btn_promo_step3').removeCls('selected');
        container.down('#btn_promo_step4').removeCls('selected');
        container.down('#btn_promo_step6').removeCls('selected');

        var jspData = $(container.down('panel[name=basicPromo]').getTargetEl().dom).data('jsp');
        var el = $(container.down('promoevent[itemId=promo_step5]').getTargetEl().dom);
        jspData.scrollToElement(el, true, true);
        button.addClass('selected');
    },

    onPromoButtonStep6Click: function (button) {
        var container = button.up('window').down('container[name=promo]');

        container.down('#btn_promo_step1').removeCls('selected');
        container.down('#btn_promo_step2').removeCls('selected');
        container.down('#btn_promo_step3').removeCls('selected');
        container.down('#btn_promo_step4').removeCls('selected');
        container.down('#btn_promo_step5').removeCls('selected');

        var jspData = $(container.down('panel[name=basicPromo]').getTargetEl().dom).data('jsp');
        var el = $(container.down('promosettings[itemId=promo_step6]').getTargetEl().dom);
        jspData.scrollToElement(el, true, true);
        button.addClass('selected');
    },

    // promo budgets steps
    onPromoBudgetsButtonStep1Click: function (button) {
        var container = button.up('window').down('container[name=promoBudgets]')

        container.down('#btn_promoBudgets_step2').removeCls('selected');
        container.down('#btn_promoBudgets_step3').removeCls('selected');

        var jspData = $(container.down('panel[name=promoBudgetsContainer]').getTargetEl().dom).data('jsp');
        jspData.scrollToY(0, true);
        button.addClass('selected');
    },

    onPromoBudgetsButtonStep2Click: function (button) {
        var container = button.up('window').down('container[name=promoBudgets]')

        container.down('#btn_promoBudgets_step1').removeCls('selected');
        container.down('#btn_promoBudgets_step3').removeCls('selected');

        var jspData = $(container.down('panel[name=promoBudgetsContainer]').getTargetEl().dom).data('jsp');
        var el = $(container.down('panel[itemId=promoBudgets_step2]').getTargetEl().dom);
        jspData.scrollToElement(el, true, true);
        button.addClass('selected');
    },

    onPromoBudgetsButtonStep3Click: function (button) {
        var container = button.up('window').down('container[name=promoBudgets]')

        container.down('#btn_promoBudgets_step1').removeCls('selected');
        container.down('#btn_promoBudgets_step2').removeCls('selected');

        var jspData = $(container.down('panel[name=promoBudgetsContainer]').getTargetEl().dom).data('jsp');
        var el = $(container.down('panel[itemId=promoBudgets_step3]').getTargetEl().dom);
        jspData.scrollToElement(el, true, true);
        button.addClass('selected');
    },

    // promo activity
    onPromoActivityButtonStep1Click: function (button) {
        var container = button.up('window').down('container[name=promoActivity]')

        container.down('#btn_promoActivity_step2').removeCls('selected');
        var jspData = $(container.down('panel[name=promoActivityContainer]').getTargetEl().dom).data('jsp');
        var el = $(container.down('panel[itemId=promoActivity_step1]').getTargetEl().dom);
        jspData.scrollToElement(el, true, true);
        button.addClass('selected');
    },

    onPromoActivityButtonStep2Click: function (button) {
        var container = button.up('window').down('container[name=promoActivity]')

        container.down('#btn_promoActivity_step1').removeCls('selected');
        var jspData = $(container.down('panel[name=promoActivityContainer]').getTargetEl().dom).data('jsp');
        var el = $(container.down('panel[itemId=promoActivity_step2]').getTargetEl().dom);
        jspData.scrollToElement(el, true, true);
        button.addClass('selected');
    },

    // support steps
    onSupportButtonStep1Click: function (button) {
        var container = button.up('window').down('container[name=support]')

        container.down('#btn_support_step2').removeCls('selected');
        container.down('#btn_support_step3').removeCls('selected');

        var jspData = $(container.down('panel[name=supportContainer]').getTargetEl().dom).data('jsp');
        var el = $(container.down('panel[itemId=support_step1] customtoptreetoolbar').getTargetEl().dom);
        jspData.scrollToElement(el, true, true);
        button.addClass('selected');
    },

    onSupportButtonStep2Click: function (button) {
        var container = button.up('window').down('container[name=support]')

        container.down('#btn_support_step1').removeCls('selected');
        container.down('#btn_support_step3').removeCls('selected');

        var jspData = $(container.down('panel[name=supportContainer]').getTargetEl().dom).data('jsp');
        var el = $(container.down('panel[itemId=support_step2] customtoptreetoolbar').getTargetEl().dom);
        jspData.scrollToElement(el, true, true);
        button.addClass('selected');
    },

    onSupportButtonStep3Click: function (button) {
        var container = button.up('window').down('container[name=support]')

        container.down('#btn_support_step1').removeCls('selected');
        container.down('#btn_support_step2').removeCls('selected');

        var jspData = $(container.down('panel[name=supportContainer]').getTargetEl().dom).data('jsp');
        var el = $(container.down('panel[itemId=support_step3] customtoptreetoolbar').getTargetEl().dom);
        jspData.scrollToElement(el, true, true);
        button.addClass('selected');
    },

    // =============== Promo buttons ===============

    onCreateButtonClick: function (button, e, schedulerData, isInOutPromo) {
        var me = this;
        var promoeditorcustom = Ext.widget('promoeditorcustom');
        promoeditorcustom.isCreating = true;
        // из-за вызова из календаря, нужно конкретизировать
        this.getController('tpm.promo.Promo').detailButton = null;
        promoeditorcustom.isFromSchedule = schedulerData;

        // для блокировки/разблокировки грида/календаря
        // Если кнопка не null, то из грида, иначе из календаря
        var parentWidget = button ? button.up('promo') : Ext.ComponentQuery.query('schedulecontainer')[0];
        parentWidget.setLoading(true);

        if (schedulerData && schedulerData.isCopy) {
            this.bindAllLoadEvents(promoeditorcustom, schedulerData, true);
            me.fillPromoForm(promoeditorcustom, schedulerData, false, true);
        } else {
            this.getScrollPosition(button);

            this.bindAllLoadEvents(promoeditorcustom, null, false);
            $.ajax({
                dataType: 'json',
                url: '/odata/PromoStatuss',
                success: function (promoStatusData) {
                    var client = promoeditorcustom.down('container[name=promo_step1]');
                    var product = promoeditorcustom.down('container[name=promo_step2]');
                    var mechanic = promoeditorcustom.down('container[name=promo_step3]');
                    var period = promoeditorcustom.down('container[name=promo_step4]');
                    var event = promoeditorcustom.down('container[name=promo_step5]');
                    var settings = promoeditorcustom.down('container[name=promo_step6]');

                    // Кнопки для изменения состояний промо
                    var promoActions = Ext.ComponentQuery.query('button[isPromoAction=true]');

                    promoeditorcustom.down('button[itemId=btn_promoBudgets]').setDisabled(true);
                    promoeditorcustom.down('button[itemId=btn_promoBudgets]').addCls('disabled');

                    promoeditorcustom.down('button[itemId=btn_promoActivity]').setDisabled(true);
                    promoeditorcustom.down('button[itemId=btn_promoActivity]').addCls('disabled');

                    // mechanic
                    var promoController = App.app.getController('tpm.promo.Promo');
                    var marsMechanicId = mechanic.down('searchcombobox[name=MarsMechanicId]');
                    var instoreMechanicId = mechanic.down('searchcombobox[name=PlanInstoreMechanicId]');
                    var marsMechanicTypeId = mechanic.down('searchcombobox[name=MarsMechanicTypeId]');
                    var instoreMechanicTypeId = mechanic.down('searchcombobox[name=PlanInstoreMechanicTypeId]');
                    var marsMechanicDiscount = mechanic.down('numberfield[name=MarsMechanicDiscount]');
                    var instoreMechanicDiscount = mechanic.down('numberfield[name=PlanInstoreMechanicDiscount]');
                    var promoComment = mechanic.down('textarea[name=PromoComment]');

                    promoController.mechanicTypeChange(
                        marsMechanicId, marsMechanicTypeId, marsMechanicDiscount,
                        promoController.getMechanicListForUnlockDiscountField()
                    );

                    promoController.mechanicTypeChange(
                        instoreMechanicId, instoreMechanicTypeId, instoreMechanicDiscount,
                        promoController.getMechanicListForUnlockDiscountField()
                    );

                    // event
                    event.down('chooseEventButton').updateMappingValues(new App.model.tpm.event.Event({
                        Id: null,
                        Name: 'Standard promo',
                        Year: 0,
                        Period: '',
                        Description: ''
                    }))

                    // settings
                    settings.down('sliderfield[name=priority]').setValue(3);
                    var promoEventButton = Ext.ComponentQuery.query('button[itemId=btn_promo_step6]')[0];
                    promoEventButton.setText('<b>' + l10n.ns('tpm', 'promoStap').value('basicStep6') + '</b><br><p> Calendar priority: ' + 3 + '</p>');
                    promoEventButton.removeCls('notcompleted');
                    promoEventButton.setGlyph(0xf133);

                    // если создание из календаря
                    if (schedulerData) {
                        var promoClientForm = promoeditorcustom.down('container[name=promo_step1]');
                        var durationDateStart = period.down('datefield[name=DurationStartDate]');
                        var durationDateEnd = period.down('datefield[name=DurationEndDate]');
                        var startDate = schedulerData.schedulerContext.start;
                        var endDate = schedulerData.schedulerContext.end;
                        var clientRecord;

                        if (schedulerData.isCopy) { }
                        else {
                            clientRecord = schedulerData.schedulerContext.resourceRecord.raw;
                        }

                        durationDateStart.setValue(startDate);
                        durationDateEnd.setValue(endDate);

                        if (clientRecord) {
                            promoClientForm.fillForm(clientRecord, false);
                            me.checkParametersAfterChangeClient(clientRecord, promoeditorcustom);
                            me.afterInitClient(clientRecord, schedulerData.schedulerContext.resourceRecord, promoeditorcustom, schedulerData.isCopy);
                        }
                    }

                    for (var i = 0; i < promoStatusData.value.length; i++) {
                        if (promoStatusData.value[i].SystemName == 'Draft') {
                            promoeditorcustom.statusId = promoStatusData.value[i].Id;
                            promoeditorcustom.promoStatusName = promoStatusData.value[i].Name;
                            promoeditorcustom.promoStatusSystemName = promoStatusData.value[i].SystemName;
                            promoeditorcustom.promoName = 'Unpublish Promo';

                            me.setPromoTitle(promoeditorcustom, promoeditorcustom.promoName, promoeditorcustom.promoStatusName);
                            me.defineAllowedActions(promoeditorcustom, promoActions, promoeditorcustom.promoStatusName);

                            var undoBtn = promoeditorcustom.down('button[itemId=btn_undoPublish]');
                            undoBtn.statusId = promoStatusData.value[i].Id;
                            undoBtn.statusName = promoStatusData.value[i].Name;
                            undoBtn.statusSystemName = promoStatusData.value[i].SystemName;
                        }

                        if (promoStatusData.value[i].SystemName == 'DraftPublished') {
                            var btn = promoeditorcustom.down('button[itemId=btn_publish]');
                            btn.statusId = promoStatusData.value[i].Id;
                            btn.statusName = promoStatusData.value[i].Name;
                            btn.statusSystemName = promoStatusData.value[i].SystemName;
                        }

                        if (promoStatusData.value[i].SystemName == 'OnApproval') {
                            var btn = promoeditorcustom.down('button[itemId=btn_sendForApproval]');
                            btn.statusId = promoStatusData.value[i].Id;
                            btn.statusName = promoStatusData.value[i].Name;
                            btn.statusSystemName = promoStatusData.value[i].SystemName;
                        }

                        if (promoStatusData.value[i].SystemName == 'Approved') {
                            var btn_approve = promoeditorcustom.down('button[itemId=btn_approve]');
                            btn_approve.statusId = promoStatusData.value[i].Id;
                            btn_approve.statusName = promoStatusData.value[i].Name;
                            btn_approve.statusSystemName = promoStatusData.value[i].SystemName;
                        }

                        if (promoStatusData.value[i].SystemName == 'Cancelled') {
                            var btn_cancel = promoeditorcustom.down('button[itemId=btn_cancel]');
                            btn_cancel.statusId = promoStatusData.value[i].Id;
                            btn_cancel.statusName = promoStatusData.value[i].Name;
                            btn_cancel.statusSystemName = promoStatusData.value[i].SystemName;
                        }

                        if (promoStatusData.value[i].SystemName == 'Planned') {
                            var btn_plan = promoeditorcustom.down('button[itemId=btn_plan]');
                            btn_plan.statusId = promoStatusData.value[i].Id;
                            btn_plan.statusName = promoStatusData.value[i].Name;
                            btn_plan.statusSystemName = promoStatusData.value[i].SystemName;
                        }

                        if (promoStatusData.value[i].SystemName == 'Closed') {
                            var btn_close = promoeditorcustom.down('button[itemId=btn_close]');
                            btn_close.statusId = promoStatusData.value[i].Id;
                            btn_close.statusName = promoStatusData.value[i].Name;
                            btn_close.statusSystemName = promoStatusData.value[i].SystemName;
                        }

                        if (promoStatusData.value[i].SystemName == 'Finished') {
                            var btn_backToFinished = promoeditorcustom.down('button[itemId=btn_backToFinished]');
                            btn_backToFinished.statusId = promoStatusData.value[i].Id;
                            btn_backToFinished.statusName = promoStatusData.value[i].Name;
                            btn_backToFinished.statusSystemName = promoStatusData.value[i].SystemName;
                        }
                    }

                    promoeditorcustom.show();
                    parentWidget.setLoading(false);
                    me.checkLoadingComponents();
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    parentWidget.setLoading(false);
                    App.Notify.pushError(l10n.ns('tpm', 'text').value('failedStatusLoad'));
                }
            });
        }
    },

    onCreateInOutButtonClick: function (button, e, schedulerData) {
        var me = this;
        me.onCreateButtonClick(button, e, schedulerData, true);
    },
    onUpdateButtonClick: function (button) {
        var me = this;
        var grid = this.getGridByButton(button);
        grid.up('promo').setLoading(true);
        var promoeditorcustom = Ext.widget('promoeditorcustom');
        me.detailButton = null;
        var promoStatusName = null;

        // Если запись обозначена
        if (button.assignedRecord) {
            promoeditorcustom.isCreating = false;
            promoeditorcustom.assignedRecord = button.assignedRecord;

            this.bindAllLoadEvents(promoeditorcustom, button.assignedRecord, false);
            this.fillPromoForm(promoeditorcustom, button.assignedRecord, false, false, grid);

            promoStatusName = button.assignedRecord.get('PromoStatusName');
            // Редактирование выбранной записи
        } else {
            me.getScrollPosition(button);

            var selectionModel = grid.getSelectionModel();
            if (selectionModel.hasSelection()) {
                var record = selectionModel.getSelection()[0];
                grid.promoStore.load({
                    id: record.getId(),
                    scope: me,
                    callback: function (records, operation, success) {
                        if (success) {
                            record = records[0];
                            promoeditorcustom.isCreating = false;
                            promoeditorcustom.model = record;
                            me.bindAllLoadEvents(promoeditorcustom, record, false);
                            me.fillPromoForm(promoeditorcustom, record, false, false, grid);
                            promoStatusName = record.get('PromoStatusName');
                        }
                    }
                });
            } else {
                grid.up('promo').setLoading(false);
                App.Notify.pushError(l10n.ns('tpm', 'text').value('notSelected'));
            }
        }

        //Для блокирования кнопки продуктов
        promoeditorcustom.productsSetted = true;


        //Установка readOnly полям, для которых текущая роль не входит в crudAccess
        me.setFieldsReadOnlyForSomeRole(promoeditorcustom);

        var needRecountUplift = Ext.ComponentQuery.query('[itemId=PromoUpliftLockedUpdateCheckbox]')[0];
        var planUplift = Ext.ComponentQuery.query('[name=PlanPromoUpliftPercent]')[0];
        needRecountUplift.setDisabled(false);

        if (needRecountUplift.value === true) {
            planUplift.setReadOnly(false);
            planUplift.removeCls('readOnlyField');
        } else {
            planUplift.setReadOnly(true);
            planUplift.addCls('readOnlyField');
        }

        //Начавшиеся promo не редактируются период
        var isPromoWasStarted = (['Started', 'Finished', 'Closed'].indexOf(promoStatusName) >= 0);
        if (isPromoWasStarted) {
            me.blockStartedPromoDateChange(promoeditorcustom, me);
        }

        me.setFieldsReadOnlyForSomeRole(promoeditorcustom);
    },

    onDetailButtonClick: function (button) {
        var me = this;
        var promoeditorcustom = Ext.widget('promoeditorcustom');
        me.getController('tpm.promo.Promo').detailButton = button;

        // Если запись обозначена
        if (button.assignedRecord) {
            promoeditorcustom.isCreating = false;
            promoeditorcustom.assignedRecord = button.assignedRecord;

            me.bindAllLoadEvents(promoeditorcustom, button.assignedRecord, false);
            me.fillPromoForm(promoeditorcustom, button.assignedRecord, true);
            // Просмотр выбранной записи
        } else {
            this.getScrollPosition(button);

            var grid = me.getGridByButton(button);
            var selectionModel = grid.getSelectionModel();
            if (selectionModel.hasSelection()) {
                var record = selectionModel.getSelection()[0];
                grid.promoStore.load({
                    id: record.getId(),
                    scope: me,
                    callback: function (records, operation, success) {
                        if (success) {
                            record = records[0];
                            promoeditorcustom.isCreating = false;
                            promoeditorcustom.model = record;
                            me.bindAllLoadEvents(promoeditorcustom, record, false);
                            me.fillPromoForm(promoeditorcustom, record, true, false, grid);
                        }
                    }
                });
            } else {
                App.Notify.pushError(l10n.ns('tpm', 'text').value('notSelected'));
            }
            var cancelButton = promoeditorcustom.down('#cancelPromo');
            cancelButton.promoGridDetailMode = grid;
        }
    },

    onSavePromoButtonClick: function (button) {
        this.savePromo(button, false, true);
    },

    onSaveAndClosePromoButtonClick: function (button) {
        this.savePromo(button, true);
    },

    // Кнопка редактирования промо (Edit button)
    onChangePromoButtonClick: function (button) {
        var window = button.up('promoeditorcustom');
        window.setLoading(true);
        // отложенный вызов функции изменения состояния формы для того чтобы сначала отрисовался лоадер
        Ext.Function.defer(this.setEditingAfterSetLoading, 1, this, [button]);
    },

    // Перевод формы промо в режим редактирования
    setEditingAfterSetLoading: function (button) {
        var me = this,
            promoeditorcustom = button.up('window');
        promoeditorcustom.readOnly = false;

        var promoActivity = promoeditorcustom.down('promoactivity');

        var promoClientForm = promoeditorcustom.down('container[name=promo_step1]');
        var promoProductForm = promoeditorcustom.down('container[name=promo_step2]');
        var mechanic = promoeditorcustom.down('container[name=promo_step3]');
        var promoActivityStep1 = promoActivity.down('container[name=promoActivity_step1]');

        var currentRole = App.UserInfo.getCurrentRole()['SystemName'];

        // --------------- basic promo ---------------

        // Promo Client
        var clientCrudAccess = ['Administrator', 'FunctionalExpert', 'CMManager', 'CustomerMarketing', 'KeyAccountManager'];
        if (clientCrudAccess.indexOf(currentRole) > -1) {
            var window = button.up('promoeditorcustom');

            promoClientForm.down('#choosePromoClientBtn').setDisabled(false);
        }

        // Product tree
        var productCrudAccess = ['Administrator', 'FunctionalExpert', 'CMManager', 'CustomerMarketing', 'KeyAccountManager'];
        if (productCrudAccess.indexOf(currentRole) > -1) {
            window.productsSetted = true;
            promoProductForm.setDisabledBtns(false);
        }

        // --------------- buttons ---------------

        //uplift
        promoeditorcustom.down('button[itemId=savePromo]').show();
        promoeditorcustom.down('button[itemId=saveAndClosePromo]').show();
        promoeditorcustom.down('button[itemId=cancelPromo]').show();
        promoeditorcustom.down('button[itemId=closePromo]').hide();
        promoeditorcustom.down('button[itemId=changePromo]').hide();

        //Установка readOnly полям, для которых текущая роль не входит в crudAccess
        me.setFieldsReadOnlyForSomeRole(promoeditorcustom);

        var needRecountUplift = Ext.ComponentQuery.query('[itemId=PromoUpliftLockedUpdateCheckbox]')[0];
        var planUplift = Ext.ComponentQuery.query('[name=PlanPromoUpliftPercent]')[0];

        needRecountUplift.setDisabled(false);

        if (needRecountUplift.value === true) {
            planUplift.setReadOnly(false);
            planUplift.removeCls('readOnlyField');
        } else {
            planUplift.setReadOnly(true);
            planUplift.addCls('readOnlyField');
        }

        me.validatePromoModel(promoeditorcustom);

        // Разблокировка кнопок Add Promo Support
        var promoBudgets = button.up('window').down('promobudgets');
        var addSubItemButtons = promoBudgets.query('#addSubItem');

        addSubItemButtons.forEach(function (button) {
            button.setDisabled(false);
        });

        // Сброc полей Instore Mechanic
        var promoController = App.app.getController('tpm.promo.Promo'),
            promoMechanic = Ext.ComponentQuery.query('promomechanic')[0],
            mechanicFields = promoController.getMechanicFields(promoMechanic);

        promoController.disableFields([
            mechanicFields.instoreMechanicFields.instoreMechanicTypeId,
            mechanicFields.instoreMechanicFields.instoreMechanicDiscount
        ]);

        mechanicFields.instoreMechanicFields.instoreMechanicTypeId,
            mechanicFields.instoreMechanicFields.instoreMechanicDiscount

        //Установка полей механик
        var marsMechanicId = mechanic.down('searchcombobox[name=MarsMechanicId]');
        var instoreMechanicId = mechanic.down('searchcombobox[name=PlanInstoreMechanicId]');
        var marsMechanicTypeId = mechanic.down('searchcombobox[name=MarsMechanicTypeId]');
        var instoreMechanicTypeId = mechanic.down('searchcombobox[name=PlanInstoreMechanicTypeId]');
        var marsMechanicDiscount = mechanic.down('numberfield[name=MarsMechanicDiscount]');
        var instoreMechanicDiscount = mechanic.down('numberfield[name=PlanInstoreMechanicDiscount]');

        promoController.mechanicTypeChange(
            marsMechanicId, marsMechanicTypeId, marsMechanicDiscount,
            promoController.getMechanicListForUnlockDiscountField()
        );

        promoController.mechanicTypeChange(
            instoreMechanicId, instoreMechanicTypeId, instoreMechanicDiscount,
            promoController.getMechanicListForUnlockDiscountField()
        );

        //Actual механники
        var actualInstoreMechanicId = promoActivityStep1.down('searchcombobox[name=ActualInstoreMechanicId]');
        var actualInstoreMechanicTypeId = promoActivityStep1.down('searchcombobox[name=ActualInstoreMechanicTypeId]');
        var actualInStoreDiscount = promoActivityStep1.down('numberfield[name=ActualInStoreDiscount]');

        promoController.mechanicTypeChange(
            actualInstoreMechanicId, actualInstoreMechanicTypeId, actualInStoreDiscount,
            promoController.getMechanicListForUnlockDiscountField()
        );

        //Начавшиеся promo не редактируются период
        var isPromoWasStarted = (['Started', 'Finished', 'Closed'].indexOf(promoeditorcustom.promoStatusName) >= 0);
        if (isPromoWasStarted) {
            me.blockStartedPromoDateChange(promoeditorcustom, me);
        }
        me.checkLoadingComponents();
        promoeditorcustom.setLoading(false);
    },

    blockStartedPromoDateChange: function (promoeditorcustom, me) {
        var period = promoeditorcustom.down('container[name=promo_step4]');
        var durationStartDate = period.down('datefield[name=DurationStartDate]');
        var durationEndDate = period.down('datefield[name=DurationEndDate]');
        var dispatchStartDate = period.down('datefield[name=DispatchStartDate]');
        var dispatchEndDate = period.down('datefield[name=DispatchEndDate]');
        var dateFields = [durationStartDate, durationEndDate, dispatchStartDate, dispatchEndDate];
        me.setReadOnlyProperty(true, dateFields);
        for (var i = 0; i < dateFields.length; i++) {
            dateFields[i].setReadOnly(true);
            dateFields[i].addCls('readOnlyField');
        }
    },

    onCancelPromoButtonClick: function (button) {
        var window = button.up('promoeditorcustom');
        var promoeditorcustom = button.up('window');
        promoeditorcustom.setLoading(true, undefined, true);
        var me = this;
        setTimeout(function () {

            // Если есть открытый календарь - обновить его
            var calendarGrid = Ext.ComponentQuery.query('scheduler');
            if (calendarGrid.length > 0) {
                calendarGrid[0].resourceStore.load();
            }

            if (window) {
                var grid = button.promoGridDetailMode;
                if (grid) {
                    //var selectionModel = grid.getSelectionModel();
                    //if (selectionModel.hasSelection()) {
                    //    var record = selectionModel.getSelection()[0];
                    //    window.readOnly = true;
                    //    me.reFillPromoForm(window, record, grid);
                    //}
                    window.readOnly = true;
                    me.reFillPromoForm(window, window.model, grid);
                } else {
                    window.close();
                }
            }
        }, 5);
        promoeditorcustom.setLoading(false, undefined, false);
    },

    onClosePromoButtonClick: function (button) {
        var window = button.up('promoeditorcustom');

        // Если есть открытый календарь - обновить его
        var calendarGrid = Ext.ComponentQuery.query('scheduler');
        if (calendarGrid.length > 0) {
            calendarGrid[0].resourceStore.load();
        }
        if (window) {
            window.close();
        }
    },

    onPromoHistoryButtonClick: function (button) {
        var choosepromowindow = Ext.create('App.view.core.base.BaseReviewWindow', {
            title: l10n.ns('tpm', 'customtoptoolbar').value('customHistory'),
            width: "95%",
            height: "95%",
            minWidth: 800,
            minHeight: 600,
            layout: 'fit',
            id: 'promoHistoryChanges',
            items: [{
                xtype: 'customhistoricalpromo'
            }]
        });

        var window = button.up('window');
        var grid = choosepromowindow.down('grid');
        var store = grid.getStore();

        store.setFixedFilter('HistoricalObjectId', {
            property: '_ObjectId',
            operation: 'Equals',
            value: window.promoId
        });

        store.on({
            load: function (records, operation, success) {
                var selModel = grid.getSelectionModel();

                if (!selModel.hasSelection() && records.data.length > 0) {
                    selModel.select(0);
                    grid.fireEvent('itemclick', grid, grid.getSelectionModel().getLastSelected());
                } else if (selModel.hasSelection() && records.data.length > 0) {
                    var selected = selModel.getSelection()[0];
                    if (store.indexOfId(selected.getId()) === -1) {
                        selModel.select(0);
                        grid.fireEvent('itemclick', grid, grid.getSelectionModel().getLastSelected());
                    }
                } else if (records.data.length === 0) {
                    selModel.deselectAll();
                }
            }
        });

        store.load();
        choosepromowindow.show();

        //Корректировка области прокрутки
        var h = choosepromowindow.down('[itemId=datatable]').getHeight();
        choosepromowindow.down('custompromopanel').setHeight(h - 34);
    },


    onPublishButtonClick: function (button) {
        var window = button.up('promoeditorcustom');
        var checkValid = this.validatePromoModel(window);
        if (checkValid==='') {
                var record = this.getRecord(window);

            window.previousStatusId = window.statusId;
            window.statusId = button.statusId;
            window.promoName = this.getPromoName(window);

            var model = this.buildPromoModel(window, record);
            this.saveModel(model, window, false, true);
        } else {
            App.Notify.pushInfo(checkValid);
        }
    },

    onUndoPublishButtonClick: function (button) {
        var window = button.up('promoeditorcustom');
        var checkValid = this.validatePromoModel(window);
        if (checkValid === '') {
            var record = this.getRecord(window);

            window.down('#PromoUpliftLockedUpdateCheckbox').setValue(false);
            window.down('[name = PlanPromoUpliftPercent]').setValue(null);

            window.previousStatusId = window.statusId;
            window.statusId = button.statusId;
            window.promoName = 'Unpublish Promo';
            window.down('#btn_recalculatePromo').hide();

            var model = this.buildPromoModel(window, record);
            this.saveModel(model, window, false, true);

            // если во время возврата была открыта вкладка Calculations/Activity нужно переключиться с них
            var btn_promo = button.up('window').down('container[name=promo]');
            //var btn_support = button.up('window').down('container[name=support]');

            if (!btn_promo.hasCls('selected')) {// && !btn_support.hasCls('selected')) {
                this.onPromoButtonClick(btn_promo);
            }
        } else {
            App.Notify.pushInfo(checkValid);
        }
    },

    onSendForApprovalButtonClick: function (button) {
        var window = button.up('promoeditorcustom');

        var checkValid = this.validatePromoModel(window);

        var isStep7Complete = !window.down('#btn_promoBudgets_step1').hasCls('notcompleted');
        var isStep8Complete = !window.down('#btn_promoBudgets_step2').hasCls('notcompleted');
        var isStep9Complete = !window.down('#btn_promoBudgets_step3').hasCls('notcompleted');

        if (checkValid === '' && isStep7Complete && isStep8Complete && isStep9Complete) {
                var record = this.getRecord(window);

            window.previousStatusId = window.statusId;
            window.statusId = button.statusId;
            window.promoName = this.getPromoName(window);

            var model = this.buildPromoModel(window, record);
            this.saveModel(model, window, false, true);
        } else {
            App.Notify.pushInfo(checkValid);
        }
    },

    onApproveButtonClick: function (button) {
        var window = button.up('promoeditorcustom');
        var checkValid = this.validatePromoModel(window);

        // TODO: необходимо точно выяснить ограничения
        var isStep7Complete = !window.down('#btn_promoBudgets_step1').hasCls('notcompleted');
        var isStep8Complete = !window.down('#btn_promoBudgets_step2').hasCls('notcompleted');
        var isStep9Complete = !window.down('#btn_promoBudgets_step3').hasCls('notcompleted');

        if (checkValid === '' && (isStep7Complete && isStep8Complete && isStep9Complete)) {
            var me = this;
            // окно подтверждения
            Ext.Msg.show({
                title: l10n.ns('tpm', 'text').value('Confirmation'),
                msg: l10n.ns('tpm', 'Promo').value('Confirm Approval'),
                fn: function (btn) {
                    if (btn === 'yes') {
                        // Логика для согласования
                        var record = me.getRecord(window);

                        window.previousStatusId = window.statusId;
                        window.statusId = button.statusId;
                        window.promoName = me.getPromoName(window);

                        var model = me.buildPromoModel(window, record);

                        // если есть доступ, то через сохранение (нужно протестировать механизмы)
                        var pointsAccess = App.UserInfo.getCurrentRole().AccessPoints;
                        var access = pointsAccess.find(function (element) {
                            return (element.Resource == 'Promoes' && element.Action == 'Patch') || 
                                   (element.Resource == 'PromoGridViews' && element.Action == 'Patch');
                        });

                        if (access) {
                            me.saveModel(model, window, false, true);
                        }
                        else {
                            me.changeStatusPromo(record.data.Id, button.statusId, window);
                        }
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
        } else {
            App.Notify.pushInfo(checkValid);
        }
    },

    onCancelButtonClick: function (button) {
        var window = button.up('promoeditorcustom');
        var record = this.getRecord(window);
        window.previousStatusId = window.statusId;
        window.statusId = button.statusId;
        window.promoName = this.getPromoName(window);
        var model = this.buildPromoModel(window, record);
        window.readOnly = true;
        this.saveModel(model, window, false, true);
        window.down('#btn_showlog').hide();
        window.down('#btn_recalculatePromo').hide();
        window.down('#changePromo').hide();
        window.down('#cancelPromo').hide();
        window.down('#closePromo').show();
    },

    onPlanButtonClick: function (button) {
        var window = button.up('promoeditorcustom');

        var checkValid = this.validatePromoModel(window);

        // TODO: необходимо точно выяснить ограничения
        var isStep7Complete = !window.down('#btn_promoBudgets_step1').hasCls('notcompleted');
        var isStep8Complete = !window.down('#btn_promoBudgets_step2').hasCls('notcompleted');
        var isStep9Complete = !window.down('#btn_promoBudgets_step3').hasCls('notcompleted');

        if (checkValid === '' && (isStep7Complete && isStep8Complete && isStep9Complete)) {
                var record = this.getRecord(window);
                var me = this;

            window.previousStatusId = window.statusId;
            window.statusId = button.statusId;
            window.promoName = this.getPromoName(window);

            var model = this.buildPromoModel(window, record);

            // если есть доступ, то через сохранение (нужно протестировать механизмы)
            var pointsAccess = App.UserInfo.getCurrentRole().AccessPoints;
            var access = pointsAccess.find(function (element) {
                return element.Resource == 'Promoes' && element.Action == 'Patch';
            });

            if (access) {
                me.saveModel(model, window, false, true);
            }
            else {
                me.changeStatusPromo(record.data.Id, button.statusId, window);
            }
        } else {
            App.Notify.pushInfo(checkValid);
        }
    },


    onToClosePromoButtonClick: function (button) {
        var window = button.up('promoeditorcustom');

        // TODO: необходимо точно выяснить ограничения
        var isStep7Complete = !window.down('#btn_promoBudgets_step1').hasCls('notcompleted');
        var isStep8Complete = !window.down('#btn_promoBudgets_step2').hasCls('notcompleted');
        var isStep9Complete = !window.down('#btn_promoBudgets_step3').hasCls('notcompleted');

        var CheckValid = this.validatePromoModel(window);

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
        var actIN = promoactivity.down('textfield[name = InvoiceNumber]');
        var actINIsValid = !(actIN.value === "");
        var isActivityPromoValid = actMIsValid && actAISPIsValid && actINIsValid;
        if (CheckValid === '' && !isActivityPromoValid) {
            CheckValid = 'In order to close promo Actual In Store and Invoice Number in Activity must be filled.';
        }

        if ((CheckValid === '') && (isStep7Complete && isStep8Complete && isStep9Complete) && (isPromoValid && isActivityPromoValid)) {
            var record = this.getRecord(window);
            var me = this;

            window.previousStatusId = window.statusId;
            window.statusId = button.statusId;
            window.promoName = this.getPromoName(window);

            var model = this.buildPromoModel(window, record);

            // если есть доступ, то через сохранение (нужно протестировать механизмы)
            var pointsAccess = App.UserInfo.getCurrentRole().AccessPoints;
            var access = pointsAccess.find(function (element) {
                return element.Resource == 'Promoes' && element.Action == 'Patch';
            });

            if (access) {
                me.saveModel(model, window, false, true);
            }
            else {
                me.changeStatusPromo(record.data.Id, button.statusId, window);
            }
        } else {
            App.Notify.pushInfo(CheckValid);
        }
    },

    onBackToFinishedPromoButtonClick: function (button) {
        var window = button.up('promoeditorcustom');

        var checkValid = this.validatePromoModel(window);

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
                var record = this.getRecord(window);
                var me = this;

                window.previousStatusId = window.statusId;
                window.statusId = button.statusId;
                window.promoName = this.getPromoName(window);

                var model = this.buildPromoModel(window, record);

                // если есть доступ, то через сохранение (нужно протестировать механизмы)
                var pointsAccess = App.UserInfo.getCurrentRole().AccessPoints;
                var access = pointsAccess.find(function (element) {
                    return element.Resource == 'Promoes' && element.Action == 'Patch';
                });

                me.changeStatusPromo(record.data.Id, button.statusId, window);
            } else {
                return;
            }
        } else {
            App.Notify.pushInfo(checkValid);
        }
    },

    // =============== Other ===============

    //Общая валидация всех полей промо
    validatePromoModel: function (window) {
        var errorMessage = '';
        var errorSecondLayer = '';
        var errorTrirdLayer = '';

        var isStep1Complete = !window.down('#btn_promo_step1').hasCls('notcompleted');
        var isStep2Complete = !window.down('#btn_promo_step2').hasCls('notcompleted');

        var promomechanic = window.down('promomechanic');
        var promoperiod = window.down('promoperiod');
        var promosettings = window.down('promosettings');
        var promoactivity = window.down('promoactivity');
        var currentStatusName = window.promoStatusName;

        //Basic
        //Step 1
        if (!isStep1Complete)
            errorSecondLayer += l10n.ns('tpm', 'text').value('completeStep1Validate') + "; ";
        //Step 2
        if (!isStep2Complete)
            errorSecondLayer += l10n.ns('tpm', 'text').value('completeStep2Validate') + "; ";
        //Step 3
        if (!promomechanic.down('searchcombobox[name=MarsMechanicId]').validate()
            || !promomechanic.down('searchcombobox[name=MarsMechanicTypeId]').validate()
            || !promomechanic.down('numberfield[name=MarsMechanicDiscount]').validate())
            errorTrirdLayer += l10n.ns('tpm', 'text').value('MarsMechanicValidate') + ", ";

        if (!promomechanic.down('searchcombobox[name=PlanInstoreMechanicTypeId]').validate()
            || !promomechanic.down('numberfield[name=PlanInstoreMechanicDiscount]').validate())
            errorTrirdLayer += l10n.ns('tpm', 'text').value('PlanInstoreMechanicValidate') + ", ";

        if (!promomechanic.down('textarea[name=PromoComment]').validate())
            errorTrirdLayer += l10n.ns('tpm', 'text').value('PromoCommentValidate') + ", ";

        if (errorTrirdLayer != '' || window.down('#btn_promo_step3').hasCls('notcompleted')) {
            if (errorTrirdLayer.endsWith(', ')) errorTrirdLayer = errorTrirdLayer.replace(/..$/, '; ');
            errorSecondLayer += l10n.ns('tpm', 'text').value('completeStep3Validate') + ' - ' + errorTrirdLayer;
            errorTrirdLayer = '';
        }

        //Step 4
        //Promo в статусе Started и после не проверяются на текущую дату
        if (['Started', 'Finished', 'Closed'].indexOf(currentStatusName) < 0) {
            if (!promoperiod.down('datefield[name=DurationStartDate]').validate()
                || !promoperiod.down('datefield[name=DurationEndDate]').validate())
                errorTrirdLayer += l10n.ns('tpm', 'text').value('DurationDateValidate') + ", ";

            if (!promoperiod.down('datefield[name=DispatchStartDate]').validate()
                || !promoperiod.down('datefield[name=DispatchEndDate]').validate())
                errorTrirdLayer += l10n.ns('tpm', 'text').value('DispatchValidate') + ", ";

            if (errorTrirdLayer != '' || window.down('#btn_promo_step4').hasCls('notcompleted')) {
                if (errorTrirdLayer.endsWith(', ')) errorTrirdLayer = errorTrirdLayer.replace(/..$/, '; ');
                errorSecondLayer += l10n.ns('tpm', 'text').value('completeStep4Validate') + ' - ' + errorTrirdLayer;
                errorTrirdLayer = '';
            }
        }

        //Step 5
        if (window.down('#btn_promo_step5').hasCls('notcompleted')) {
            errorSecondLayer += l10n.ns('tpm', 'text').value('completeStep5Validate') + "; ";
        }

        //Step 6
        if (window.down('#btn_promo_step6').hasCls('notcompleted')) {
            errorSecondLayer += l10n.ns('tpm', 'text').value('completeStep5Validate') + "; ";
        }

        //End basic validation
        if (errorSecondLayer != '') {
            if (errorSecondLayer.endsWith('; ')) errorSecondLayer = errorSecondLayer.replace(/..$/, '. ');
            errorMessage += l10n.ns('tpm', 'text').value('completeBasicValidate') + ': ' + errorSecondLayer;
            errorSecondLayer = ''
        }

        //В статусе Draft Activity заблокировано, нет смысла валидировать
        if (['Draft'].indexOf(currentStatusName) < 0) {
            //Activity - step 1
            if (!promoactivity.down('searchcombobox[name=ActualInstoreMechanicId]').validate()
                || !promoactivity.down('searchcombobox[name=ActualInstoreMechanicTypeId]').validate()
                || !promoactivity.down('numberfield[name=ActualInStoreDiscount]').validate()
                || !promoactivity.down('numberfield[name=ActualInStoreShelfPrice]').validate())
                errorTrirdLayer += l10n.ns('tpm', 'text').value('ActualInstoreAssumptionValidate') + ", ";

            if (!promoactivity.down('numberfield[name=PlanInStoreShelfPrice]').validate())
                errorTrirdLayer += l10n.ns('tpm', 'text').value('PlanInstoreAssumptionValidate') + ", ";

            if (errorTrirdLayer != '') {
                if (errorTrirdLayer.endsWith(', ')) errorTrirdLayer = errorTrirdLayer.replace(/..$/, '; ');
                errorSecondLayer += l10n.ns('tpm', 'text').value('completeStep1Validate') + ' - ' + errorTrirdLayer;
                errorTrirdLayer = '';
            }

            //Activity - step 2
            if (!promoactivity.down('textfield[name=InvoiceNumber]').validate()) {
                errorTrirdLayer += l10n.ns('tpm', 'text').value('InvoiceNumberValidate') + ", ";
            }
            if (errorTrirdLayer != '') {
                if (errorTrirdLayer.endsWith(', ')) errorTrirdLayer = errorTrirdLayer.replace(/..$/, '; ');
                errorSecondLayer += l10n.ns('tpm', 'text').value('completeStep2Validate') + ' - ' + errorTrirdLayer;
                errorTrirdLayer = '';
            }

            //End activity validation
            if (errorSecondLayer != '') {
                if (errorSecondLayer.endsWith('; ')) errorSecondLayer = errorSecondLayer.replace(/..$/, '. ');
                errorMessage += l10n.ns('tpm', 'text').value('completeActivityValidate') + ': ' + errorSecondLayer;
                errorSecondLayer = ''
            }
        }

        //End validation
        if (errorMessage != '')
            return l10n.ns('tpm', 'text').value('completeStepsValidate') + errorMessage;

        else return errorMessage;
    },

    buildPromoModel: function (window, record) {
        // basic promo
        var promomechanic = window.down('promomechanic');
        var promoperiod = window.down('promoperiod');
        var promoevent = window.down('promoevent');
        var promosettings = window.down('promosettings');

        // promo budgets
        var promoBudgets = window.down('promobudgets');

        // calculation
        //var promocalculation = window.down('promocalculation');

        // promo activity
        var promoActivity = window.down('promoactivity');

        // --------------- basic promo ---------------
        record.data.PromoStatusId = window.statusId;
        record.data.ClientId = null;
        record.data.Name = window.promoName;

        // promo client
        record.data.ClientTreeId = window.clientTreeId;
        record.data.ClientHierarchy = window.clientHierarchy;
        record.data.ClientTreeKeyId = window.clientTreeKeyId;

        // producttree
        record.data.BrandId = window.brandId;
        record.data.TechnologyId = window.technologyId;
        record.data.ProductHierarchy = window.productHierarchy;

        record.data.ProductTreeObjectIds = '';
        window.productTreeNodes.forEach(function (objectId, index) {
            record.data.ProductTreeObjectIds += objectId;

            if (index != window.productTreeNodes.length - 1)
                record.data.ProductTreeObjectIds += ';';
        });

        // promomechanic
        var marsMechanicId = promomechanic.down('searchcombobox[name=MarsMechanicId]').getValue();
        var marsMechanicTypeId = promomechanic.down('searchcombobox[name=MarsMechanicTypeId]').getValue();
        var marsMechanicDiscount = promomechanic.down('numberfield[name=MarsMechanicDiscount]').getValue();

        var instoreMechanicId = promomechanic.down('searchcombobox[name=PlanInstoreMechanicId]').getValue();
        var instoreMechanicTypeId = promomechanic.down('searchcombobox[name=PlanInstoreMechanicTypeId]').getValue();
        var instoreMechanicDiscount = promomechanic.down('numberfield[name=PlanInstoreMechanicDiscount]').getValue();

        record.data.MechanicComment = promomechanic.down('textarea[name=PromoComment]').getValue();

        record.data.MarsMechanicId = marsMechanicId ? marsMechanicId : null;
        record.data.MarsMechanicTypeId = marsMechanicTypeId ? marsMechanicTypeId : null;
        record.data.MarsMechanicDiscount = marsMechanicDiscount ? marsMechanicDiscount : null;

        record.data.PlanInstoreMechanicId = instoreMechanicId ? instoreMechanicId : null;
        record.data.PlanInstoreMechanicTypeId = instoreMechanicTypeId ? instoreMechanicTypeId : null;
        record.data.PlanInstoreMechanicDiscount = instoreMechanicDiscount ? instoreMechanicDiscount : null;

        // promoperiod
        record.data.StartDate = promoperiod.down('datefield[name=DurationStartDate]').getValue();
        record.data.EndDate = promoperiod.down('datefield[name=DurationEndDate]').getValue();
        record.data.DispatchesStart = promoperiod.down('datefield[name=DispatchStartDate]').getValue();
        record.data.DispatchesEnd = promoperiod.down('datefield[name=DispatchEndDate]').getValue();

        // promoevent
        record.data.EventId = promoevent.down('chooseEventButton').getValue();

        // promosettings
        record.data.CalendarPriority = promosettings.down('sliderfield[name=priority]').getValue();

        // --------------- promo budgets ---------------
        var totalCostBudgets = promoBudgets.down('container[name=promoBudgets_step1]');

        // cost and budget
        record.data.PlanPromoTIShopper = totalCostBudgets.down('numberfield[name=PlanPromoTIShopper]').getValue();
        record.data.PlanPromoTIMarketing = totalCostBudgets.down('numberfield[name=PlanPromoTIMarketing]').getValue();
        record.data.PlanPromoBranding = totalCostBudgets.down('numberfield[name=PlanPromoBranding]').getValue();
        record.data.PlanPromoCost = totalCostBudgets.down('numberfield[name=PlanPromoCost]').getValue();
        record.data.PlanPromoBTL = totalCostBudgets.down('numberfield[name=PlanPromoBTL]').getValue();
        record.data.PlanPromoCostProduction = totalCostBudgets.down('numberfield[name=PlanPromoCostProduction]').getValue();

        record.data.ActualPromoTIShopper = totalCostBudgets.down('numberfield[name=ActualPromoTIShopper]').getValue();
        record.data.ActualPromoTIMarketing = totalCostBudgets.down('numberfield[name=ActualPromoTIMarketing]').getValue();
        record.data.ActualPromoBranding = totalCostBudgets.down('numberfield[name=ActualPromoBranding]').getValue();
        record.data.ActualPromoCost = totalCostBudgets.down('numberfield[name=ActualPromoCost]').getValue();
        record.data.ActualPromoBTL = totalCostBudgets.down('numberfield[name=ActualPromoBTL]').getValue();
        record.data.ActualPromoCostProduction = totalCostBudgets.down('numberfield[name=ActualPromoCostProduction]').getValue();

        // --------------- calculation ---------------

        //var activity = promocalculation.down('container[name=activity]');
        //var financialIndicator = promocalculation.down('container[name=financialIndicator]');

        // activity
        //record.data.PlanPromoUpliftPercent = activity.down('numberfield[name=PlanPromoUpliftPercent]').getValue();
        //record.data.PlanPromoIncrementalLSV = activity.down('numberfield[name=PlanPromoIncrementalLSV]').getValue();
        //record.data.PlanPromoIncrementalLSV = activity.down('numberfield[name=PlanPromoIncrementalLSV]').getValue();
        //record.data.PlanPromoPostPromoEffectLSV = activity.down('trigger[name=PlanPostPromoEffectTotal]').getValue();

        //record.data.ActualPromoUpliftPercent = activity.down('numberfield[name=ActualPromoUpliftPercent]').getValue();
        //record.data.ActualPromoIncrementalLSV = activity.down('numberfield[name=ActualPromoIncrementalLSV]').getValue();
        //record.data.ActualPromoLSV = activity.down('numberfield[name=ActualPromoLSV]').getValue();
        //record.data.ActualPromoPostPromoEffectLSV = activity.down('trigger[name=FactPostPromoEffectTotal]').getValue();

        //// financial indicator
        //record.data.PlanPromoROIPercent = financialIndicator.down('numberfield[name=PlanPromoROIPercent]').getValue();
        //record.data.PlanPromoIncrementalNSV = financialIndicator.down('numberfield[name=PlanPromoIncrementalNSV]').getValue();
        //record.data.PlanPromoNetIncrementalNSV = financialIndicator.down('numberfield[name=PlanPromoNetIncrementalNSV]').getValue();
        //record.data.PlanPromoIncrementalMAC = financialIndicator.down('numberfield[name=PlanPromoIncrementalMAC]').getValue();

        //record.data.ActualPromoROIPercent = financialIndicator.down('numberfield[name=ActualPromoROIPercent]').getValue();
        //record.data.ActualPromoIncrementalNSV = financialIndicator.down('numberfield[name=ActualPromoIncrementalNSV]').getValue();
        //record.data.ActualPromoNetIncrementalNSV = financialIndicator.down('numberfield[name=ActualPromoNetIncrementalNSV]').getValue();
        //record.data.ActualPromoIncrementalMAC = financialIndicator.down('numberfield[name=ActualPromoIncrementalMAC]').getValue();

        // --------------- promo activity ---------------

        var promoActivityStep1 = promoActivity.down('container[name=promoActivity_step1]');
        var promoActivityStep2 = promoActivity.down('container[name=promoActivity_step2]');

        record.data.ActualInStoreMechanicId = promoActivityStep1.down('searchcombobox[name=ActualInstoreMechanicId]').getValue();
        record.data.ActualInStoreMechanicTypeId = promoActivityStep1.down('searchcombobox[name=ActualInstoreMechanicTypeId]').getValue();
        record.data.ActualInStoreDiscount = promoActivityStep1.down('numberfield[name=ActualInStoreDiscount]').getValue();

        record.data.ActualInStoreShelfPrice = promoActivityStep1.down('numberfield[name=ActualInStoreShelfPrice]').getValue();
        record.data.PlanInStoreShelfPrice = promoActivityStep1.down('numberfield[name=PlanInStoreShelfPrice]').getValue();

        record.data.InvoiceNumber = promoActivityStep2.down('textfield[name=InvoiceNumber]').getValue();
        record.data.PlanPromoUpliftPercent = promoActivityStep2.down('numberfield[name=PlanPromoUpliftPercent]').getValue();

        var needRecountUplift = promoActivityStep2.down('#PromoUpliftLockedUpdateCheckbox').getValue();
        if (needRecountUplift === true) {
            record.data.NeedRecountUplift = false;
        } else {
            record.data.NeedRecountUplift = true;
        }
        //record.data.PlanPromoBaselineLSV = promoActivityStep2.down('numberfield[name=PlanPromoBaselineLSV]').getValue();
        //record.data.PlanPromoIncrementalLSV = promoActivityStep2.down('numberfield[name=PlanPromoIncrementalLSV]').getValue();
        //record.data.PlanPromoLSV = promoActivityStep2.down('numberfield[name=PlanPromoLSV]').getValue();
        //record.data.PlanPostPromoEffectTotal = promoActivityStep2.down('numberfield[name=PlanPostPromoEffectTotal]').getValue();

        //record.data.ActualPromoUpliftPercent = promoActivityStep2.down('numberfield[name=ActualPromoUpliftPercent]').getValue();
        //record.data.ActualPromoBaselineLSV = promoActivityStep2.down('numberfield[name=ActualPromoBaselineLSV]').getValue();
        //record.data.ActualPromoIncrementalLSV = promoActivityStep2.down('numberfield[name=ActualPromoIncrementalLSV]').getValue();
        //record.data.ActualPromoLSV = promoActivityStep2.down('numberfield[name=ActualPromoLSVByCompensation]').getValue();
        //record.data.FactPostPromoEffectTotal = promoActivityStep2.down('numberfield[name=FactPostPromoEffectTotal]').getValue();
        return record;
    },

    getBaseClientTreeId: function (treegrid, item) {
        if (item.get('IsBaseClient')) {
            return item.get('ObjectId')
        } else {
            // Идем наверх по дереву, пока не найдем базового клиента
            var store = treegrid.getStore(),
                parent = store.getNodeById(item.get('parentId'));
            if (parent) {
                while (parent.data.root !== true) {
                    if (parent.get('IsBaseClient')) {
                        return parent.get('ObjectId')
                    } else {
                        parent = store.getNodeById(parent.get('parentId'));
                    }
                }
            }
        }
        return null;
    },

    fillPromoForm: function (promoeditorcustom, record, readOnly, isCopy, promoGrid) {
        var me = this;
        // для блокировки/разблокировки грида/календаря
        // Если кнопка не null, то из грида, иначе из календаря
        var parentWidget = promoGrid ? promoGrid.up('promo') : Ext.ComponentQuery.query('schedulecontainer')[0];
        parentWidget.setLoading(true);
        // т.к. при открытии промо больше нет ожидания загрузки статусов, нужно сначала повесить лоадер
        Ext.Function.defer(me.fillPromoFormAfterSetLoading, 1, me, [promoeditorcustom, record, readOnly, isCopy, parentWidget]);
    },

    //вызывается отложенно из fillPromoForm для корректной отрисовки лоадера
    fillPromoFormAfterSetLoading: function (promoeditorcustom, record, readOnly, isCopy, parentWidget) {
        var me = this,
            calculating = record.get('Calculating');

        readOnly = readOnly || calculating;
        promoeditorcustom.readOnly = readOnly;
        $.ajax({
            dataType: 'json',
            url: '/odata/PromoStatuss',
            success: function (promoStatusData) {
                var draftStatusData = null;
                for (var i = 0; i < promoStatusData.value.length; i++) {
                    if (promoStatusData.value[i].Id == record.data.PromoStatusId) {
                        promoeditorcustom.statusId = promoStatusData.value[i].Id;
                        promoeditorcustom.promoStatusName = promoStatusData.value[i].Name;
                        promoeditorcustom.promoStatusSystemName = promoStatusData.value[i].SystemName;
                    }

                    if (promoStatusData.value[i].SystemName == 'DraftPublished') {
                        var btn_publish = promoeditorcustom.down('button[itemId=btn_publish]');
                        btn_publish.statusId = promoStatusData.value[i].Id;
                        btn_publish.statusName = promoStatusData.value[i].Name;
                        btn_publish.statusSystemName = promoStatusData.value[i].SystemName;
                    }

                    if (promoStatusData.value[i].SystemName == 'OnApproval') {
                        var btn_sendForApproval = promoeditorcustom.down('button[itemId=btn_sendForApproval]');
                        btn_sendForApproval.statusId = promoStatusData.value[i].Id;
                        btn_sendForApproval.statusName = promoStatusData.value[i].Name;
                        btn_sendForApproval.statusSystemName = promoStatusData.value[i].SystemName;
                    }

                    if (promoStatusData.value[i].SystemName == 'Approved') {
                        var btn_approve = promoeditorcustom.down('button[itemId=btn_approve]');
                        btn_approve.statusId = promoStatusData.value[i].Id;
                        btn_approve.statusName = promoStatusData.value[i].Name;
                        btn_approve.statusSystemName = promoStatusData.value[i].SystemName;
                    }

                    if (promoStatusData.value[i].SystemName == 'Cancelled') {
                        var btn_cancel = promoeditorcustom.down('button[itemId=btn_cancel]');
                        btn_cancel.statusId = promoStatusData.value[i].Id;
                        btn_cancel.statusName = promoStatusData.value[i].Name;
                        btn_cancel.statusSystemName = promoStatusData.value[i].SystemName;
                    }

                    if (promoStatusData.value[i].SystemName == 'Planned') {
                        var btn_plan = promoeditorcustom.down('button[itemId=btn_plan]');
                        btn_plan.statusId = promoStatusData.value[i].Id;
                        btn_plan.statusName = promoStatusData.value[i].Name;
                        btn_plan.statusSystemName = promoStatusData.value[i].SystemName;
                    }

                    if (promoStatusData.value[i].SystemName == 'Closed') {
                        var btn_close = promoeditorcustom.down('button[itemId=btn_close]');
                        btn_close.statusId = promoStatusData.value[i].Id;
                        btn_close.statusName = promoStatusData.value[i].Name;
                        btn_close.statusSystemName = promoStatusData.value[i].SystemName;
                    }

                    if (promoStatusData.value[i].SystemName == 'Finished') {
                        var btn_backToFinished = promoeditorcustom.down('button[itemId=btn_backToFinished]');
                        btn_backToFinished.statusId = promoStatusData.value[i].Id;
                        btn_backToFinished.statusName = promoStatusData.value[i].Name;
                        btn_backToFinished.statusSystemName = promoStatusData.value[i].SystemName;
                    }

                    if (promoStatusData.value[i].SystemName == 'Draft') {
                        draftStatusData = promoStatusData.value[i];

                        var undoBtn = promoeditorcustom.down('button[itemId=btn_undoPublish]');
                        undoBtn.statusId = promoStatusData.value[i].Id;
                        undoBtn.statusName = promoStatusData.value[i].Name;
                        undoBtn.statusSystemName = promoStatusData.value[i].SystemName;
                    }
                }
                if (isCopy) {
                    promoeditorcustom.statusId = draftStatusData.Id;
                    promoeditorcustom.promoStatusName = draftStatusData.Name;
                }
            },
            error: function () {
                parentWidget.setLoading(false);
                App.Notify.pushError(l10n.ns('tpm', 'text').value('failedStatusLoad'));
            }
        });

        var customtoptoolbar = promoeditorcustom.down('customtoptoolbar');
        var promoController = App.app.getController('tpm.promo.Promo');

        // --------------- basic promo ---------------
        var promoClientForm = promoeditorcustom.down('container[name=promo_step1]');
        var promoProductForm = promoeditorcustom.down('container[name=promo_step2]');

        var mechanic = promoeditorcustom.down('container[name=promo_step3]');
        var period = promoeditorcustom.down('container[name=promo_step4]');
        var event = promoeditorcustom.down('container[name=promo_step5]');
        var settings = promoeditorcustom.down('container[name=promo_step6]');

        // mechanic
        var marsMechanicId = mechanic.down('searchcombobox[name=MarsMechanicId]');
        var instoreMechanicId = mechanic.down('searchcombobox[name=PlanInstoreMechanicId]');
        var marsMechanicTypeId = mechanic.down('searchcombobox[name=MarsMechanicTypeId]');
        var instoreMechanicTypeId = mechanic.down('searchcombobox[name=PlanInstoreMechanicTypeId]');
        var marsMechanicDiscount = mechanic.down('numberfield[name=MarsMechanicDiscount]');
        var instoreMechanicDiscount = mechanic.down('numberfield[name=PlanInstoreMechanicDiscount]');
        var promoComment = mechanic.down('textarea[name=PromoComment]');

        // period
        var durationStartDate = period.down('datefield[name=DurationStartDate]');
        var durationEndDate = period.down('datefield[name=DurationEndDate]');
        var dispatchStartDate = period.down('datefield[name=DispatchStartDate]');
        var dispatchEndDate = period.down('datefield[name=DispatchEndDate]');

        // event
        var promoEvent = event.down('chooseEventButton');

        //settings
        var priority = settings.down('sliderfield[name=priority]');

        // --------------- promo budgets ---------------

        var promoBudgets = promoeditorcustom.down('promobudgets');

        // total cost budgets
        var totalCostBudgets = promoBudgets.down('container[name=promoBudgets_step1]');

        var shopperTi = totalCostBudgets.down('numberfield[name=PlanPromoTIShopper]');
        var marketingTi = totalCostBudgets.down('numberfield[name=PlanPromoTIMarketing]');
        var branding = totalCostBudgets.down('numberfield[name=PlanPromoBranding]');
        var totalCost = totalCostBudgets.down('numberfield[name=PlanPromoCost]');
        var btl = totalCostBudgets.down('numberfield[name=PlanPromoBTL]');
        var costProduction = totalCostBudgets.down('numberfield[name=PlanPromoCostProduction]');

        var actualPromoTIShopper = totalCostBudgets.down('numberfield[name=ActualPromoTIShopper]');
        var actualPromoTIMarketing = totalCostBudgets.down('numberfield[name=ActualPromoTIMarketing]');
        var actualPromoBranding = totalCostBudgets.down('numberfield[name=ActualPromoBranding]');
        var factTotalCost = totalCostBudgets.down('numberfield[name=ActualPromoCost]');
        var factBtl = totalCostBudgets.down('numberfield[name=ActualPromoBTL]');
        var factCostProduction = totalCostBudgets.down('numberfield[name=ActualPromoCostProduction]');

        // marketing TI
        var marketingTIStep = promoBudgets.down('container[name=promoBudgets_step2]');

        var planPromoXSites = marketingTIStep.down('triggerfield[name=budgetDet-PlanX-sites]');
        var planPromoCatalogue = marketingTIStep.down('triggerfield[name=budgetDet-PlanCatalog]');
        var planPromoPOSMInClient = marketingTIStep.down('triggerfield[name=budgetDet-PlanPOSM]');

        var actualPromoXSites = marketingTIStep.down('triggerfield[name=budgetDet-ActualX-sites]');
        var actualPromoCatalogue = marketingTIStep.down('triggerfield[name=budgetDet-ActualCatalog]');
        var actualPromoPOSMInClient = marketingTIStep.down('triggerfield[name=budgetDet-ActualPOSM]');

        // cost production
        var costProductionStep = promoBudgets.down('container[name=promoBudgets_step3]');

        var planPromoCostProdXSites = costProductionStep.down('triggerfield[name=budgetDet-PlanCostProdX-sites]');
        var planPromoCostProdCatalogue = costProductionStep.down('triggerfield[name=budgetDet-PlanCostProdCatalog]');
        var planPromoCostProdPOSMInClient = costProductionStep.down('triggerfield[name=budgetDet-PlanCostProdPOSM]');

        var actualPromoCostProdXSites = costProductionStep.down('triggerfield[name=budgetDet-ActualCostProdX-sites]');
        var actualPromoCostProdCatalogue = costProductionStep.down('triggerfield[name=budgetDet-ActualCostProdCatalog]');
        var actualPromoCostProdPOSMInClient = costProductionStep.down('triggerfield[name=budgetDet-ActualCostProdPOSM]');

        // --------------- promo activity ---------------

        var promoActivity = promoeditorcustom.down('promoactivity');
        var promoActivityStep1 = promoActivity.down('container[name=promoActivity_step1]');
        var promoActivityStep2 = promoActivity.down('container[name=promoActivity_step2]');

        var actualInstoreMechanicId = promoActivityStep1.down('searchcombobox[name=ActualInstoreMechanicId]');
        var actualInstoreMechanicTypeId = promoActivityStep1.down('searchcombobox[name=ActualInstoreMechanicTypeId]');
        var actualInStoreDiscount = promoActivityStep1.down('numberfield[name=ActualInStoreDiscount]');

        var actualInStoreShelfPrice = promoActivityStep1.down('numberfield[name=ActualInStoreShelfPrice]');

        var invoiceNumber = promoActivityStep2.down('textfield[name=InvoiceNumber]');
        var planPromoUpliftPercent = promoActivityStep2.down('[name=PlanPromoUpliftPercent]');
        var promoUpliftLockedUpdateCheckbox = promoActivityStep2.down('checkbox[itemId=PromoUpliftLockedUpdateCheckbox]');
        var planPromoBaselineLSV = promoActivityStep2.down('[name=PlanPromoBaselineLSV]');
        var planPromoIncrementalLSV = promoActivityStep2.down('[name=PlanPromoIncrementalLSV]');
        var planPromoLSV = promoActivityStep2.down('[name=PlanPromoLSV]');
        var planPromoLSV = promoActivityStep2.down('[name=PlanPromoLSV]');
        var planPostPromoEffect = promoActivityStep2.down('[name=PlanPromoPostPromoEffectLSV]');

        var actualPromoUpliftPercent = promoActivityStep2.down('[name=ActualPromoUpliftPercent]');
        var actualPromoBaselineLSV = promoActivityStep2.down('[name=ActualPromoBaselineLSV]');
        var actualPromoIncrementalLSV = promoActivityStep2.down('[name=ActualPromoIncrementalLSV]');
        var actualPromoLSV = promoActivityStep2.down('[name=ActualPromoLSV]');
        var actualPromoLSVbyCompensation = promoActivityStep2.down('[name=ActualPromoLSVByCompensation]');
        var factPostPromoEffect = promoActivityStep2.down('[name=ActualPromoPostPromoEffectLSV]');

        // Блокировка изменения значений
        if (readOnly) {
            // --------------- basic promo ---------------
            // client
            promoClientForm.down('#choosePromoClientBtn').setDisabled(true);

            // product
            promoProductForm.setDisabledBtns(true);

            // event
            promoEvent.setDisabled(true); // button

            // settings
            priority.setReadOnly(true);

            var elementsToReadOnly = promoeditorcustom.query('[needReadOnly=true]');
            me.setReadOnlyProperty(true, elementsToReadOnly);

            // --------------- buttons ---------------
            promoeditorcustom.down('button[itemId=savePromo]').hide();
            promoeditorcustom.down('button[itemId=saveAndClosePromo]').hide();
            promoeditorcustom.down('button[itemId=cancelPromo]').hide();
            promoeditorcustom.down('button[itemId=closePromo]').show();
            if (record && record.data && record.data.PromoStatusSystemName === 'Cancelled') {
                promoeditorcustom.down('button[itemId=changePromo]').hide();
            } else {
                promoeditorcustom.down('button[itemId=changePromo]').show();
            }
        }

        if (isCopy) {
            promoeditorcustom.down('button[itemId=btn_promoBudgets]').setDisabled(true);
            promoeditorcustom.down('button[itemId=btn_promoBudgets]').addCls('disabled');
            promoeditorcustom.down('button[itemId=btn_promoActivity]').setDisabled(true);
            promoeditorcustom.down('button[itemId=btn_promoActivity]').addCls('disabled');
        } else {
            promoeditorcustom.promoId = record.data.Id;
            promoeditorcustom.statusId = record.data.PromoStatusId;
            promoeditorcustom.promoName = record.data.Name;

            switch (record.data.PromoStatusSystemName) {
                case 'Draft':
                    promoeditorcustom.down('button[itemId=btn_promoBudgets]').setDisabled(true);
                    promoeditorcustom.down('button[itemId=btn_promoBudgets]').addCls('disabled');
                    promoeditorcustom.down('button[itemId=btn_promoActivity]').setDisabled(true);
                    promoeditorcustom.down('button[itemId=btn_promoActivity]').addCls('disabled');
                    break;
                case 'DraftPublished':
                    promoeditorcustom.down('button[itemId=btn_promoBudgets]').setDisabled(false);
                    promoeditorcustom.down('button[itemId=btn_promoBudgets]').removeCls('disabled');
                    promoeditorcustom.down('button[itemId=btn_promoActivity]').setDisabled(false);
                    promoeditorcustom.down('button[itemId=btn_promoActivity]').removeCls('disabled');
                    break;
                case 'OnApproval':
                    promoeditorcustom.down('button[itemId=btn_promoBudgets]').setDisabled(false);
                    promoeditorcustom.down('button[itemId=btn_promoBudgets]').removeCls('disabled');
                    promoeditorcustom.down('button[itemId=btn_promoActivity]').setDisabled(false);
                    promoeditorcustom.down('button[itemId=btn_promoActivity]').removeCls('disabled');
                    break;
                case 'Approved':
                    promoeditorcustom.down('button[itemId=btn_promoBudgets]').setDisabled(false);
                    promoeditorcustom.down('button[itemId=btn_promoBudgets]').removeCls('disabled');
                    promoeditorcustom.down('button[itemId=btn_promoActivity]').setDisabled(false);
                    promoeditorcustom.down('button[itemId=btn_promoActivity]').removeCls('disabled');
                    break;
                case 'Planned':
                    promoeditorcustom.down('button[itemId=btn_promoBudgets]').setDisabled(false);
                    promoeditorcustom.down('button[itemId=btn_promoBudgets]').removeCls('disabled');
                    promoeditorcustom.down('button[itemId=btn_promoActivity]').setDisabled(false);
                    promoeditorcustom.down('button[itemId=btn_promoActivity]').removeCls('disabled');
                    break;
                case 'Started':
                    promoeditorcustom.down('button[itemId=btn_promoBudgets]').setDisabled(false);
                    promoeditorcustom.down('button[itemId=btn_promoBudgets]').removeCls('disabled');
                    promoeditorcustom.down('button[itemId=btn_promoActivity]').setDisabled(false);
                    promoeditorcustom.down('button[itemId=btn_promoActivity]').removeCls('disabled');
                    break;
                case 'Finished':
                    promoeditorcustom.down('button[itemId=btn_promoBudgets]').setDisabled(false);
                    promoeditorcustom.down('button[itemId=btn_promoBudgets]').removeCls('disabled');
                    promoeditorcustom.down('button[itemId=btn_promoActivity]').setDisabled(false);
                    promoeditorcustom.down('button[itemId=btn_promoActivity]').removeCls('disabled');
                    break;
                case 'Closed':
                    promoeditorcustom.down('button[itemId=btn_promoBudgets]').setDisabled(false);
                    promoeditorcustom.down('button[itemId=btn_promoBudgets]').removeCls('disabled');
                    promoeditorcustom.down('button[itemId=btn_promoActivity]').setDisabled(false);
                    promoeditorcustom.down('button[itemId=btn_promoActivity]').removeCls('disabled');
                    break;
                default:
                    promoeditorcustom.down('button[itemId=btn_promoBudgets]').setDisabled(true);
                    promoeditorcustom.down('button[itemId=btn_promoBudgets]').addCls('disabled');
                    promoeditorcustom.down('button[itemId=btn_promoActivity]').setDisabled(true);
                    promoeditorcustom.down('button[itemId=btn_promoActivity]').addCls('disabled');
                    break;
            }
        }

        var titleText = isCopy ?
            Ext.String.format('Name: {0}, Status: {1}', promoeditorcustom.promoName, promoeditorcustom.promoStatusName) :
            Ext.String.format('Name: {0}, Status: {1}', record.data.Name, record.data.PromoStatusName);

        customtoptoolbar.down('label[name=promoName]').setText(titleText);

        // Деревья client и Product забираются по дате при статусе Approval для статусов после и при Approved
        var treesChangingBlockDate = ['Draft', 'DraftPublished', 'OnApproval'].indexOf(record.data.PromoStatusSystemName) < 0 ? record.get('LastApprovedDate') : null;
        if (treesChangingBlockDate) {
            treesChangingBlockDate = Ext.Date.format(treesChangingBlockDate, 'Y-m-d\\TH:i:s');
        }

        // client
        // Если запись создаётся копированием, клиент берётся из календаря, а не из копируемой записи        
        var clientRecord = isCopy ? record.schedulerContext.resourceRecord.raw : record.raw.ClientTree;

        if (clientRecord) {
            promoClientForm.fillForm(clientRecord, treesChangingBlockDate);
        }

        var currentRole = App.UserInfo.getCurrentRole()['SystemName'];
        var clientCrudAccess = ['Administrator', 'FunctionalExpert', 'CMManager', 'CustomerMarketing', 'KeyAccountManager'];

        if (clientCrudAccess.indexOf(currentRole) > -1) {
            promoClientForm.down('#choosePromoClientBtn').setDisabled(readOnly);
        } else {
            promoClientForm.down('#choosePromoClientBtn').setDisabled(true);
        }

        // product
        promoProductForm.fillFormJson(record.data.PromoBasicProducts, treesChangingBlockDate);
        if (record.data.PromoBasicProducts)
            me.setInfoPromoBasicStep2(promoProductForm);

        var productCrudAccess = ['Administrator', 'FunctionalExpert', 'CMManager', 'CustomerMarketing', 'KeyAccountManager'];
        if (productCrudAccess.indexOf(currentRole) > -1) {
            promoProductForm.setDisabledBtns(readOnly);
        } else {
            promoProductForm.setDisabledBtns(true);
        }

        // mechanic
        marsMechanicId.setValue(new App.model.tpm.mechanic.Mechanic({
            Id: record.data.MarsMechanicId,
            Name: record.data.MarsMechanicName
        }));
        if (record.data.MarsMechanicTypeId) {
            marsMechanicTypeId.setValue(new App.model.tpm.mechanictype.MechanicType({
                Id: record.data.MarsMechanicTypeId,
                Name: record.data.MarsMechanicTypeName
            }));
            marsMechanicDiscount.setValue(record.data.MarsMechanicDiscount);
        } else if (record.data.MarsMechanicDiscount) {
            marsMechanicDiscount.setValue(record.data.MarsMechanicDiscount);
            marsMechanicTypeId.clearInvalid();
        }

        if (marsMechanicId.crudAccess.indexOf(currentRole) === -1) {
            marsMechanicId.setReadOnly(true);
        }
        if (marsMechanicTypeId.crudAccess.indexOf(currentRole) === -1) {
            marsMechanicTypeId.setReadOnly(true);
        }
        if (marsMechanicDiscount.crudAccess.indexOf(currentRole) === -1) {
            marsMechanicDiscount.setReadOnly(true);
        }

        instoreMechanicId.setValue(new App.model.tpm.mechanic.Mechanic({
            Id: record.data.PlanInstoreMechanicId,
            Name: record.data.PlanInstoreMechanicName
        }));
        if (record.data.PlanInstoreMechanicTypeId) {
            instoreMechanicTypeId.setValue(new App.model.tpm.mechanictype.MechanicType({
                Id: record.data.PlanInstoreMechanicTypeId,
                Name: record.data.PlanInstoreMechanicTypeName
            }));
            instoreMechanicDiscount.setValue(record.data.PlanInstoreMechanicDiscount);
        } else if (record.data.PlanInstoreMechanicDiscount) {
            instoreMechanicDiscount.setValue(record.data.PlanInstoreMechanicDiscount);
        }

        instoreMechanicTypeId.clearInvalid();
        instoreMechanicDiscount.clearInvalid();

        if (instoreMechanicId.crudAccess.indexOf(currentRole) === -1) {
            instoreMechanicId.setReadOnly(true);
        }
        if (instoreMechanicTypeId.crudAccess.indexOf(currentRole) === -1) {
            instoreMechanicTypeId.setReadOnly(true);
        }
        if (instoreMechanicDiscount.crudAccess.indexOf(currentRole) === -1) {
            instoreMechanicDiscount.setReadOnly(true);
        }

        promoController.mechanicTypeChange(
            marsMechanicId, marsMechanicTypeId, marsMechanicDiscount,
            promoController.getMechanicListForUnlockDiscountField(),
            readOnly
        );

        promoController.mechanicTypeChange(
            instoreMechanicId, instoreMechanicTypeId, instoreMechanicDiscount,
            promoController.getMechanicListForUnlockDiscountField(),
            readOnly
        );

        promoComment.setValue(record.data.MechanicComment);

        if (promoComment.crudAccess.indexOf(currentRole) === -1) {
            promoComment.setReadOnly(true);
        }
        // period
        // Если запись создаётся копированием, даты берутся из календаря, а не из копируемой записи
        var startDate = isCopy ? record.schedulerContext.start : record.data.StartDate;
        var endDate = isCopy ? record.schedulerContext.end : record.data.EndDate;

        durationStartDate.setValue(startDate);
        durationEndDate.setValue(endDate);

        if (durationStartDate.crudAccess.indexOf(currentRole) === -1) {
            durationStartDate.setReadOnly(true);
        }
        if (durationEndDate.crudAccess.indexOf(currentRole) === -1) {
            durationEndDate.setReadOnly(true);
        }

        if (dispatchStartDate.crudAccess.indexOf(currentRole) === -1) {
            dispatchStartDate.setReadOnly(true);
        }
        if (dispatchEndDate.crudAccess.indexOf(currentRole) === -1) {
            dispatchEndDate.setReadOnly(true);
        }

        if (clientRecord) {
            me.checkParametersAfterChangeClient(clientRecord, promoeditorcustom);
            me.afterInitClient(clientRecord, record, promoeditorcustom, isCopy);
        }

        var _event = new App.model.tpm.event.Event({
            Id: record.data.EventId,
            Name: record.data.PromoEventName,
            Year: record.data.PromoEventYear,
            Period: record.data.PromoEventPeriod,
            Description: record.data.PromoEventDescription
        })

        promoEvent.setValue(_event);
        promoEvent.updateMappingValues(_event);

        if (promoEvent.crudAccess.indexOf(currentRole) === -1) {
            promoEvent.setDisabled(true);
        }

        var stateListForExcludePeriodValidation = ['Cancelled', 'Started', 'Finished', 'Closed', 'Deleted', 'Undefined'];
        stateListForExcludePeriodValidation.forEach(function (state) {
            if (state == record.data.PromoStatusSystemName) {
                durationStartDate.setMinValue(null);
                durationStartDate.setMaxValue(null);
                durationStartDate.clearInvalid();

                durationEndDate.setMinValue(null);
                durationEndDate.setMaxValue(null);
                durationEndDate.clearInvalid();
            }
        });

        // settings
        priorityValue = isCopy ? 3 : record.data.CalendarPriority;
        priority.setValue(priorityValue);

        if (priority.crudAccess.indexOf(currentRole) === -1) {
            priority.setReadOnly(true);
        }

        var promoEventButton = Ext.ComponentQuery.query('button[itemId=btn_promo_step6]')[0];
        promoEventButton.setText('<b>' + l10n.ns('tpm', 'promoStap').value('basicStep6') + '</b><br><p>' + l10n.ns('tpm', 'Promo').value('CalendarPriority') + ': ' + priorityValue + '</p>');
        promoEventButton.removeCls('notcompleted');
        promoEventButton.setGlyph(0xf133);

        // --------------- promo budgets ---------------                

        // total cost budgets
        totalCost.setValue(record.data.PlanPromoCost || 0);
        shopperTi.setValue(record.data.PlanPromoTIShopper);
        marketingTi.setValue(record.data.PlanPromoTIMarketing);
        branding.setValue(record.data.PlanPromoBranding);
        btl.setValue(record.data.PlanPromoBTL);
        costProduction.setValue(record.data.PlanPromoCostProduction);

        factTotalCost.setValue(record.data.ActualPromoCost || 0);
        actualPromoTIShopper.setValue(record.data.ActualPromoTIShopper);
        actualPromoTIMarketing.setValue(record.data.ActualPromoTIMarketing);
        actualPromoBranding.setValue(record.data.ActualPromoBranding);
        factBtl.setValue(record.data.ActualPromoBTL);
        factCostProduction.setValue(record.data.ActualPromoCostProduction);

        // marketing TI               
        planPromoXSites.setValue(record.data.PlanPromoXSites);
        planPromoCatalogue.setValue(record.data.PlanPromoCatalogue);
        planPromoPOSMInClient.setValue(record.data.PlanPromoPOSMInClient);

        actualPromoXSites.setValue(record.data.ActualPromoXSites);
        actualPromoCatalogue.setValue(record.data.ActualPromoCatalogue);
        actualPromoPOSMInClient.setValue(record.data.ActualPromoPOSMInClient);

        // cost production               
        planPromoCostProdXSites.setValue(record.data.PlanPromoCostProdXSites);
        planPromoCostProdCatalogue.setValue(record.data.PlanPromoCostProdCatalogue);
        planPromoCostProdPOSMInClient.setValue(record.data.PlanPromoCostProdPOSMInClient);

        actualPromoCostProdXSites.setValue(record.data.ActualPromoCostProdXSites);
        actualPromoCostProdCatalogue.setValue(record.data.ActualPromoCostProdCatalogue);
        actualPromoCostProdPOSMInClient.setValue(record.data.ActualPromoCostProdPOSMInClient);

        // --------------- promo activity ---------------

        actualInstoreMechanicId.setValue(new App.model.tpm.mechanic.Mechanic({
            Id: record.data.ActualInStoreMechanicId,
            Name: record.data.ActualInStoreMechanicName
        }));
        if (record.data.ActualInStoreMechanicId) {
            actualInstoreMechanicTypeId.setValue(new App.model.tpm.mechanictype.MechanicType({
                Id: record.data.ActualInStoreMechanicTypeId,
                Name: record.data.ActualInStoreMechanicTypeName
            }));
            actualInStoreDiscount.setValue(record.data.ActualInStoreDiscount);
        } else if (record.data.actualInStoreDiscount) {
            actualInStoreDiscount.setValue(record.data.ActualInStoreDiscount);
		}

		actualInstoreMechanicTypeId.clearInvalid();
		actualInStoreDiscount.clearInvalid();

        if (actualInstoreMechanicId.crudAccess.indexOf(currentRole) === -1) {
            actualInstoreMechanicId.setReadOnly(true);
        }
        if (actualInstoreMechanicTypeId.crudAccess.indexOf(currentRole) === -1) {
            actualInstoreMechanicTypeId.setReadOnly(true);
        }
        if (actualInStoreDiscount.crudAccess.indexOf(currentRole) === -1) {
            actualInStoreDiscount.setReadOnly(true);
        }

        promoController.mechanicTypeChange(
            actualInstoreMechanicId, actualInstoreMechanicTypeId, actualInStoreDiscount,
            promoController.getMechanicListForUnlockDiscountField(),
            readOnly
        );

        actualInStoreShelfPrice.setValue(record.data.ActualInStoreShelfPrice);
        invoiceNumber.setValue(record.data.InvoiceNumber);

        if (actualInStoreShelfPrice.crudAccess.indexOf(currentRole) === -1) {
            actualInStoreShelfPrice.setReadOnly(true);
        }

        if (invoiceNumber.crudAccess.indexOf(currentRole) === -1) {
            invoiceNumber.setReadOnly(true);
        }

        planPromoUpliftPercent.setValue(record.data.PlanPromoUpliftPercent);
        promoUpliftLockedUpdateCheckbox.setValue(!record.data.NeedRecountUplift);

        planPromoBaselineLSV.setValue(record.data.PlanPromoBaselineLSV);
        planPromoIncrementalLSV.setValue(record.data.PlanPromoIncrementalLSV);
        planPromoLSV.setValue(record.data.PlanPromoLSV);
        planPostPromoEffect.setValue(record.data.PlanPromoPostPromoEffectLSV);

        actualPromoUpliftPercent.setValue(record.data.ActualPromoUpliftPercent);
        actualPromoBaselineLSV.setValue(record.data.ActualPromoBaselineLSV);
        actualPromoIncrementalLSV.setValue(record.data.ActualPromoIncrementalLSV);
        actualPromoLSVbyCompensation.setValue(record.data.ActualPromoLSVByCompensation);
        actualPromoLSV.setValue(record.data.ActualPromoLSV);
        factPostPromoEffect.setValue(record.data.ActualPromoPostPromoEffectLSV);

        parentWidget.setLoading(false);

        // Кнопки для изменения состояний промо
        var promoActions = Ext.ComponentQuery.query('button[isPromoAction=true]');

        // Определяем доступные действия
        me.defineAllowedActions(promoeditorcustom, promoActions, record.data.PromoStatusSystemName);
        if (!promoeditorcustom.isVisible())
            promoeditorcustom.show();

        //если производится расчет данного промо, то необходимо сделать соотвествующие визуальные изменения окна: 
        //цвет хедера меняется на красный, кнопка Редактировать - disable = true, появляется кнопка Show Log, заблокировать кнопки смены статусов
        var toolbar = promoeditorcustom.down('customtoptoolbar');
        if (calculating) {
            toolbar.items.items.forEach(function (item, i, arr) {
                item.el.setStyle('backgroundColor', '#B53333');
                if (item.xtype == 'button' && ['btn_publish', 'btn_undoPublish', 'btn_sendForApproval', 'btn_reject', 'btn_approve', 'btn_cancel', 'btn_plan', 'btn_close', 'btn_backToFinished'].indexOf(item.itemId) > -1) {
                    item.setDisabled(true);
                }
            });
            toolbar.el.setStyle('backgroundColor', '#B53333');

            var label = toolbar.down('label[name=promoName]');
            if (label) {
                label.setText(label.text + ' — Promo is blocked for recalculations');
            }

            promoeditorcustom.down('#changePromo').setDisabled(true);
            toolbar.down('#btn_showlog').addCls('showlog');
            //toolbar.down('#btn_showlog').show();
            toolbar.down('#btn_showlog').promoId = record.data.Id;
            promoeditorcustom.down('#btn_recalculatePromo').hide();

            //me.createTaskCheckCalculation(promoeditorcustom);
        }
        else if (App.UserInfo.getCurrentRole().SystemName.toLowerCase() == 'administrator' && record.data.PromoStatusSystemName != 'Draft')
            promoeditorcustom.down('#btn_recalculatePromo').show();
        this.checkLogForErrors(record.getId());

        this.checkLoadingComponents();
    },

    saveModel: function (model, window, close, reloadPromo) {
        var grid = Ext.ComponentQuery.query('#promoGrid')[0];
        var me = this;
        var store = null;

        if (grid) {
            store = grid.down('directorygrid').promoStore;
        }

        window.setLoading(l10n.ns('core').value('savingText'));

        // останавливаем подписку на статус до загрузки окна
        if (reloadPromo)
            $.connection.logHub.server.unsubscribeStatus();

        // Response возвращается не полностью верный, mappings игнорируются
        model.save({
            success: function (response, req) {
                if (req.response.length > 0 && req.response[0].value && req.response[0].value.length > 0)
                    App.Notify.pushInfo(req.response[0].value);

                var wasCreating = window.isCreating;
                if (store) {
                    store.on({
                        single: true,
                        scope: this,
                        load: function (records, operation, success) {
                            model.set('Key');
                            //window.setLoading(false);
                            if (typeof grid.afterSaveCallback === 'function') {
                                grid.afterSaveCallback(grid);
                            }
                        }
                    });
                }
                if (close) {
                    window.setLoading(false);
                    window.close();
                } else {
                    me.currentPromoModel = model;
                    window.promoId = response.data.Id;
                    window.isCreating = false;
                    window.model = response;

                    // перезаполнить промо
                    if (reloadPromo) {
                        App.model.tpm.promo.Promo.load(response.data.Id, {
                            callback: function (newModel, operation) {
                                var showLogBtn = window.down('#btn_showlog');
                                if (showLogBtn) {
                                    showLogBtn.promoId = newModel.data.Id;
                                }

                                window.model = newModel;
                                var directorygrid = grid ? grid.down('directorygrid') : null;
                                me.reFillPromoForm(window, newModel, directorygrid);

                                // если было создано, то id был обновлен
                                if (wasCreating)
                                    me.initSignalR(window);

                                //24.06.19 Лог не показываем
                                //if (newModel.get('Calculating'))
                                //    me.onPrintPromoLog(window, grid, close);
                                //window.setLoading(false);
                            }
                        });
                    }
                    else {
                        window.setLoading(false);
                    }
                }
                // Если создание из календаря - обновляем календарь
                var scheduler = Ext.ComponentQuery.query('#nascheduler')[0];
                if (scheduler) {
                    scheduler.resourceStore.reload();
                    scheduler.eventStore.reload();
                }

                return response;
            },
            failure: function () {
                // при неудаче возвращаем старый статус
                if (window.previousStatusId) {
                    window.statusId = window.previousStatusId;
                }

                model.reject();
                window.setLoading(false);

                return null;
            }
        });
    },

    savePromo: function (button, close, reloadForm) {
        var window = button.up('promoeditorcustom');

        var isModelComplete = this.validatePromoModel(window);

        if (isModelComplete === '') {
                var record = this.getRecord(window);

            if (window.promoStatusSystemName === 'Draft') {
                this.setPromoTitle(window, window.promoName, window.promoStatusName);
            } else {
                window.promoName = this.getPromoName(window);
                this.setPromoTitle(window, window.promoName, window.promoStatusName);
            }

            var model = this.buildPromoModel(window, record);
            this.saveModel(model, window, close, reloadForm);

            // Если есть открытый календарь - обновить его
            var calendarGrid = Ext.ComponentQuery.query('scheduler');
            if (calendarGrid.length > 0) {
                calendarGrid[0].resourceStore.load();
            }
        } else {
            App.Notify.pushInfo(isModelComplete);
        }
    },

    defineAllowedActions: function (promoeditorcustom, promoActions, status) {
        var currentRole = App.UserInfo.getCurrentRole();

        for (var i = 0; i < promoActions.length; i++) {
            var roles = promoActions[i].roles;
            var statuses = promoActions[i].statuses;

            var _role = roles.find(function (element) { return element == currentRole['SystemName']; });
            var _status = statuses.find(function (element) { return element == status; });

            if (_role != null && _status != null) {
                var visible = true;

                if (_status == 'OnApproval' && promoeditorcustom.xtype == 'promoeditorcustom') {
                    var record = this.getRecord(promoeditorcustom);

                    switch (_role) {
                        case 'CMManager':
                            visible = !record.get('IsCMManagerApproved');
                            break;

                        case 'CustomerMarketing':
                            visible = !record.get('IsCMManagerApproved');
                            break;

                        case 'DemandPlanning':
                            visible = record.get('IsCMManagerApproved') && !record.get('IsDemandPlanningApproved');
                            break;

                        case 'DemandFinance':
                            visible = record.get('IsCMManagerApproved') && record.get('IsDemandPlanningApproved') && !record.get('IsDemandFinanceApproved');
                            break;

                        default: break;
                    }
                }

                promoActions[i].setVisible(visible);
            }
            else {
                promoActions[i].setVisible(false);
            }
        }
    },

    setPromoTitle: function (component, name, status) {
        var promoTitle = component.down('customtoptoolbar').down('label[name=promoName]');
        promoTitle.setText('Name: ' + name + ', Status: ' + status);
    },

    getPromoName: function (promoForm) {
        var promoProductForm = promoForm.down('promobasicproducts');
        var promoMechanic = promoForm.down('panel').down('promomechanic');

        var marsMechanic = promoMechanic.down('searchcombobox[name=MarsMechanicId]').getRawValue();
        var marsMechanicType = promoMechanic.down('searchcombobox[name=MarsMechanicTypeId]').getRawValue();
        var marsMechanicDiscountField = promoMechanic.down('[name=MarsMechanicDiscount]');
        var marsMechanicDiscountValue = marsMechanicDiscountField.getValue();

        var promoName = (promoProductForm.brandAbbreviation ? promoProductForm.brandAbbreviation : '') + ' ' + (promoProductForm.technologyAbbreviation ? promoProductForm.technologyAbbreviation : '') + ' ' +
            marsMechanic + ' ' + (marsMechanic === 'TPR' || marsMechanic === 'Other' ? marsMechanicDiscountValue + '%' : marsMechanicType);

        return promoName;
    },

    getInvalidFields: function (form) {
        var invalidFields = [];
        form.getFields().filterBy(function (field) {
            if (field.validate()) return;
            invalidFields.push(field);
        });
        return invalidFields;
    },

    showHistory: function (record, panel) {
        var model = panel.getBaseModel();

        Ext.widget('basereviewwindow', { items: Ext.create('App.view.tpm.promo.HistoricalPromo', { baseModel: model }) })
            .show().down('grid').getStore()
            .setFixedFilter('HistoricalObjectId', {
                property: '_ObjectId',
                operation: 'Equals',
                value: this.getRecordId(record)
            });
    },

    getRecordId: function (record) {
        var idProperty = record.idProperty;
        return record.get(idProperty);
    },

    setButtonState: function (window, visible) {
        window.down('#changePromo').setVisible(!visible);
        window.down('#savePromo').setVisible(visible);
        window.down('#saveAndClosePromo').setVisible(visible);
    },

    setTabsState: function (promoWindow, disabled) {
        promoWindow.down('#history').setDisabled(disabled);
        promoWindow.down('#decline').setDisabled(disabled);

        if (disabled) {
            promoWindow.down('#history').addClass('disabled');
            promoWindow.down('#decline').addClass('disabled');
        } else {
            promoWindow.down('#history').removeCls('disabled');
            promoWindow.down('#decline').removeCls('disabled');
        }

        promoWindow.down('#budget').setDisabled(disabled);
        promoWindow.down('#demand').setDisabled(disabled);
        promoWindow.down('#finance').setDisabled(disabled);
    },

    getColorEndSave: function (window, record, closeWindow, scope) {
        var picker = window.down('[name=ColorId]');
        var filter = window.down('[name=ProductFilter]').getValue();
        var parameters = {
            $actionName: 'GetSuitable',
            $method: 'POST',
            productFilter: filter
        };
        window.setLoading(true);
        breeze.EntityQuery
            .from('Colors')
            .withParameters(parameters)
            .using(Ext.ux.data.BreezeEntityManager.getEntityManager())
            .execute()
            .then(function (data) {
                window.setLoading(false);
                var result = Ext.JSON.decode(data.httpResponse.data.value);
                if (result.success) {
                    if (!result.data.isFilters && result.data.length == 1) {
                        record.set('ColorId', result.data[0]);
                        scope.saveModel(record, window, closeWindow);
                        if (!closeWindow) {
                            scope.setTabsState(window, false);
                            var promoStatusField = window.down('searchfield[name=PromoStatusId]');
                            promoStatusField.setReadOnly(false);
                        }
                    } else {
                        if (!result.data.isFilters) {
                            picker.afterPickerCreate = function (scope) {
                                scope.getStore().setFixedFilter('ColorFilter', {
                                    property: 'Id',
                                    operation: 'In',
                                    value: result.data
                                });
                            }
                        } else {
                            var filterData = result.data;
                            var filterNames = [];
                            var filters = [];
                            if (filterData.brandFilter != null && filterData.brandFilter.length > 0) {
                                filterNames.push('BrandFilter');
                                filters.push({
                                    property: 'BrandId',
                                    operation: 'In',
                                    value: filterData.brandFilter
                                });
                            };
                            if (filterData.techFilter != null && filterData.techFilter.length > 0) {
                                filterNames.push('BrandTechFilter');
                                filters.push({
                                    property: 'BrandTechId',
                                    operation: 'In',
                                    value: filterData.techFilter
                                });
                            };
                            if (filterData.subrangeFilter != null && filterData.subrangeFilter.length > 0) {
                                filterNames.push('SubrangeFilter');
                                filters.push({
                                    property: 'SubrangeId',
                                    operation: 'In',
                                    value: filterData.subrangeFilter
                                });
                            };
                            picker.afterPickerCreate = function (scope) {
                                scope.getStore().setSeveralFixedFilters(filterNames, filters);
                            }
                        }
                        picker.onSelectButtonClick = function (button) {
                            var picker = this.picker,
                                selModel = picker.down(this.selectorWidget).down('grid').getSelectionModel(),
                                selrecord = selModel.hasSelection() ? selModel.getSelection()[0] : null;

                            this.setValue(selrecord);
                            this.afterSetValue(selrecord);
                            picker.close();
                            record.set('ColorId', selrecord.getId());
                            scope.saveModel(record, window, closeWindow);
                            if (!closeWindow) {
                                scope.setTabsState(window, false);
                                var promoStatusField = window.down('searchfield[name=PromoStatusId]');
                                promoStatusField.setReadOnly(false);
                            }
                        };
                        var win = picker.createPicker();

                        if (win) {
                            win.show();
                        }
                        App.Notify.pushInfo(l10n.ns('tpm', 'message').value('ColorChoose'));
                    }
                } else {
                    App.Notify.pushError(result.message);
                }
            })
            .fail(function (data) {
                form.setLoading(false);
                App.Notify.pushError(me.getErrorMessage(data));
            });
    },

    // =============== Listeners =============== 

    // =============== MECHANIC BEGIN =============== 

    // Событие, срабатывающее при изменении поля MarsMechanicId (TPR, VP, Other, ...).
    marsMechanicListener: function (field, newValue, oldValue) {
        var promoController = App.app.getController('tpm.promo.Promo'),
            promoMechanic = field.up('promomechanic');

        if (promoMechanic) {
            var mechanicFields = promoController.getMechanicFields(promoMechanic),
                promoMechanicButton = Ext.ComponentQuery.query('button[itemId=btn_promo_step3]')[0];

            var mechanicListForUnlockDiscountField = promoController.getMechanicListForUnlockDiscountField();

            if (oldValue) {
                if (mechanicListForUnlockDiscountField.some(function (element) { return element === mechanicFields.marsMechanicFields.marsMechanicId.rawValue; })) {
                    mechanicFields.marsMechanicFields.marsMechanicTypeId.reset();
                    mechanicFields.marsMechanicFields.marsMechanicDiscount.reset();
                }
            }

            promoController.mechanicTypeChange(
                mechanicFields.marsMechanicFields.marsMechanicId,
                mechanicFields.marsMechanicFields.marsMechanicTypeId,
                mechanicFields.marsMechanicFields.marsMechanicDiscount,
                promoController.getMechanicListForUnlockDiscountField()
            );

            promoMechanicButton.setText(promoController.getFullTextForMechanicButton(promoController));
        }
    },

    // Событие, срабатывающее при изменении поля MarsMechanicTypeId (3 + 1, 4 + 1, ...).
    marsMechanicTypeListener: function (field, newValue, oldValue) {
        if (newValue !== oldValue) {
            var promoController = App.app.getController('tpm.promo.Promo'),
                promoMechanic = field.up('promomechanic');

            if (promoMechanic) {
                var mechanicFields = promoController.getMechanicFields(promoMechanic),
                    promoMechanicButton = Ext.ComponentQuery.query('button[itemId=btn_promo_step3]')[0];

                if (mechanicFields.marsMechanicFields.marsMechanicTypeId.rawValue ||
                    mechanicFields.marsMechanicFields.marsMechanicDiscount.rawValue) {

                    if (mechanicFields.marsMechanicFields.marsMechanicTypeId.rawValue) {
                        mechanicFields.marsMechanicFields.marsMechanicDiscount.setValue(
                            mechanicFields.marsMechanicFields.marsMechanicTypeId.valueModels[0].data.Discount
                        );
                    }

                    promoController.setCompletedMechanicStep(true);
                } else {
                    promoController.setNotCompletedMechanicStep(true);
                }

                promoMechanicButton.setText(promoController.getFullTextForMechanicButton(promoController));
            }
        }
    },

    // Событие, срабатывающее при изменении поля MarsMechanicDiscount (Скидка).
    marsMechanicDiscountListener: function (field, newValue, oldValue) {
        var promoController = App.app.getController('tpm.promo.Promo'),
            promoMechanic = field.up('promomechanic');

        if (promoMechanic) {
            var mechanicFields = promoController.getMechanicFields(promoMechanic),
                promoMechanicButton = Ext.ComponentQuery.query('button[itemId=btn_promo_step3]')[0];

            if (mechanicFields.marsMechanicFields.marsMechanicTypeId.rawValue ||
                mechanicFields.marsMechanicFields.marsMechanicDiscount.rawValue) {

                promoController.setCompletedMechanicStep(true);
            } else {
                promoController.setNotCompletedMechanicStep(true);
            }

            promoMechanicButton.setText(promoController.getFullTextForMechanicButton(promoController));
        }
    },

    // Событие, срабатывающее при изменении поля PlanInstoreMechanicId (TPR, VP, Other, ...).
    instoreMechanicListener: function (field, newValue, oldValue) {
        var promoController = App.app.getController('tpm.promo.Promo'),
            promoMechanic = field.up('promomechanic');

        if (promoMechanic) {
            var mechanicFields = promoController.getMechanicFields(promoMechanic),
                promoMechanicButton = Ext.ComponentQuery.query('button[itemId=btn_promo_step3]')[0];

            var mechanicListForUnlockDiscountField = promoController.getMechanicListForUnlockDiscountField();

            if (oldValue) {
                if (mechanicListForUnlockDiscountField.some(function (element) { return element === mechanicFields.instoreMechanicFields.instoreMechanicId.rawValue; })) {
                    mechanicFields.instoreMechanicFields.instoreMechanicTypeId.reset();
                    mechanicFields.instoreMechanicFields.instoreMechanicDiscount.reset();
                }
                promoController.setCompletedMechanicStep(false);
            } else {
                promoController.setNotCompletedMechanicStep(false);
            }

            promoController.mechanicTypeChange(
                mechanicFields.instoreMechanicFields.instoreMechanicId,
                mechanicFields.instoreMechanicFields.instoreMechanicTypeId,
                mechanicFields.instoreMechanicFields.instoreMechanicDiscount,
                promoController.getMechanicListForUnlockDiscountField()
            );

            promoMechanicButton.setText(promoController.getFullTextForMechanicButton(promoController));
        }
    },

    // Событие, срабатывающее при изменении поля PlanInstoreMechanicTypeId (3 + 1, 4 + 1, ...).
    instoreMechanicTypeListener: function (field, newValue, oldValue) {
        if (newValue !== oldValue) {
            var promoController = App.app.getController('tpm.promo.Promo'),
                promoMechanic = field.up('promomechanic');

            if (promoMechanic) {
                var mechanicFields = promoController.getMechanicFields(promoMechanic),
                    promoMechanicButton = Ext.ComponentQuery.query('button[itemId=btn_promo_step3]')[0];

                if (mechanicFields.instoreMechanicFields.instoreMechanicTypeId.rawValue ||
                    mechanicFields.instoreMechanicFields.instoreMechanicDiscount.rawValue) {

                    if (mechanicFields.instoreMechanicFields.instoreMechanicTypeId.rawValue) {
                        mechanicFields.instoreMechanicFields.instoreMechanicDiscount.setValue(
                            mechanicFields.instoreMechanicFields.instoreMechanicTypeId.valueModels[0].data.Discount
                        );
                    }
                    promoController.setCompletedMechanicStep(false);
                } else {
                    promoController.setNotCompletedMechanicStep(false);
                }

                promoMechanicButton.setText(promoController.getFullTextForMechanicButton(promoController));
            }
        }
    },

    // Событие, срабатывающее при изменении поля PlanInstoreMechanicDiscount (Скидка).
    instoreMechanicDiscountListener: function (field, newValue, oldValue) {
        var promoController = App.app.getController('tpm.promo.Promo'),
            promoMechanic = field.up('promomechanic'),
            promoMechanicButton = Ext.ComponentQuery.query('button[itemId=btn_promo_step3]')[0];

        if (promoMechanic) {
            var mechanicFields = promoController.getMechanicFields(promoMechanic),
                promoMechanicButton = Ext.ComponentQuery.query('button[itemId=btn_promo_step3]')[0];

            if (mechanicFields.instoreMechanicFields.instoreMechanicTypeId.rawValue ||
                mechanicFields.instoreMechanicFields.instoreMechanicDiscount.rawValue) {
                promoController.setCompletedMechanicStep(false);
            }

            promoMechanicButton.setText(promoController.getFullTextForMechanicButton(promoController));
        }
    },

    // Событие, срабатывающее при изменении поля ActualMechanicId (TPR, VP, Other, ...).
    actualMechanicListener: function (field, newValue, oldValue) {
        var promoController = App.app.getController('tpm.promo.Promo'),
            promoMechanic = field.up('#promoActivity_step1');

        if (promoMechanic) {
            var mechanicFields = promoController.getMechanicFields(promoMechanic);

            var mechanicListForUnlockDiscountField = promoController.getMechanicListForUnlockDiscountField();

            if (oldValue) {
                if (mechanicListForUnlockDiscountField.some(function (element) {
                    return element === mechanicFields.actualInstoreMechanicFields.actualInstoreMechanicId.rawValue;
                })) {
                    mechanicFields.actualInstoreMechanicFields.actualInstoreMechanicTypeId.reset();
                } else {
                    mechanicFields.actualInstoreMechanicFields.actualInStoreDiscount.reset();
                }
            }

            promoController.mechanicTypeChange(
                mechanicFields.actualInstoreMechanicFields.actualInstoreMechanicId,
                mechanicFields.actualInstoreMechanicFields.actualInstoreMechanicTypeId,
                mechanicFields.actualInstoreMechanicFields.actualInStoreDiscount,
                promoController.getMechanicListForUnlockDiscountField()
            );
        }
    },

    // Событие, срабатывающее при изменении поля ActualMechanicTypeId (3 + 1, 4 + 1, ...).
    actualMechanicTypeListener: function (field, newValue, oldValue) {
        if (newValue !== oldValue) {
            var promoController = App.app.getController('tpm.promo.Promo'),
                promoMechanic = field.up('#promoActivity_step1');

            if (promoMechanic) {
                var mechanicFields = promoController.getMechanicFields(promoMechanic);

                if (mechanicFields.actualInstoreMechanicFields.actualInstoreMechanicTypeId.rawValue ||
                    mechanicFields.actualInstoreMechanicFields.actualInStoreDiscount.rawValue) {

                    if (mechanicFields.actualInstoreMechanicFields.actualInstoreMechanicTypeId.rawValue) {
                        mechanicFields.actualInstoreMechanicFields.actualInStoreDiscount.setValue(
                            mechanicFields.actualInstoreMechanicFields.actualInstoreMechanicTypeId.valueModels[0].data.Discount
                        );
                    }
                }
            }
        }
    },

    // Событие, срабатывающее при изменении поля ActualMechanicDiscount (Скидка).
    actualMechanicDiscountListener: function (field, newValue, oldValue) {
        var promoController = App.app.getController('tpm.promo.Promo'),
            promoMechanic = field.up('#promoActivity_step1');

        if (promoMechanic) {
            var mechanicFields = promoController.getMechanicFields(promoMechanic);

            if (mechanicFields.actualInstoreMechanicFields.actualInstoreMechanicTypeId.rawValue ||
                mechanicFields.actualInstoreMechanicFields.actualInStoreDiscount.rawValue) {
            }
        }
    },

    // Вызвать при изменении поля MechanicType.
    mechanicTypeChange: function (mechanicId, mechanicTypeId, mechanicDiscount, mechanicListForUnlockDiscountField, readOnly) {
        if (readOnly) {
            mechanicId.setDisabled(false);
            mechanicTypeId.setDisabled(false);
            mechanicDiscount.setDisabled(false);
        } else if (mechanicListForUnlockDiscountField.some(function (element) { return element === mechanicId.rawValue; })) {
            mechanicTypeId.setDisabled(true);
            mechanicDiscount.setDisabled(false);
        } else if (mechanicId.rawValue) {
            mechanicTypeId.setDisabled(false);
            mechanicDiscount.setDisabled(true);
        } else {
            mechanicTypeId.setDisabled(true);
            mechanicDiscount.setDisabled(true);
        }
    },

    // Получить список механик, выбор которых может разблокировать поле MechanicDiscount и заблокировать поле MechanicTypeId.
    getMechanicListForUnlockDiscountField: function () {
        return ['TPR', 'Other'];
    },

    // Получить список полей MarsMechanic && получить список полей PlanInstoreMechanic.
    getMechanicFields: function (promoMechanicContainer) {
        return {
            marsMechanicFields: this.getMarsMechanicFields(promoMechanicContainer),
            instoreMechanicFields: this.getPlanInstoreMechanicFields(promoMechanicContainer),
            actualInstoreMechanicFields: this.getActualInstoreMechanicFields(promoMechanicContainer),
        }
    },

    // Получить список полей MarsMechanic.
    getMarsMechanicFields: function (promoMechanic) {
        return {
            marsMechanicId: promoMechanic.down('searchcombobox[name=MarsMechanicId]'),
            marsMechanicTypeId: promoMechanic.down('searchcombobox[name=MarsMechanicTypeId]'),
            marsMechanicDiscount: promoMechanic.down('numberfield[name=MarsMechanicDiscount]')
        }
    },

    // Получить список полей PlanInstoreMechanic.
    getPlanInstoreMechanicFields: function (promoMechanic) {
        return {
            instoreMechanicId: promoMechanic.down('searchcombobox[name=PlanInstoreMechanicId]'),
            instoreMechanicTypeId: promoMechanic.down('searchcombobox[name=PlanInstoreMechanicTypeId]'),
            instoreMechanicDiscount: promoMechanic.down('numberfield[name=PlanInstoreMechanicDiscount]')
        }
    },

    // Получить список полей ActualsInstoreMechanic.
    getActualInstoreMechanicFields: function (promoMechanic) {
        return {
            actualInstoreMechanicId: promoMechanic.down('searchcombobox[name=ActualInstoreMechanicId]'),
            actualInstoreMechanicTypeId: promoMechanic.down('searchcombobox[name=ActualInstoreMechanicTypeId]'),
            actualInStoreDiscount: promoMechanic.down('numberfield[name=ActualInStoreDiscount]')
        }
    },

    setNotCompletedMechanicStep: function (isMars) {
        var promoMechanicButton = Ext.ComponentQuery.query('button[itemId=btn_promo_step3]')[0];

        if (isMars) promoMechanicButton.isMarsMechanicsComplete = false;
        else promoMechanicButton.isInstoreMechanicsComplete = false;
        promoMechanicButton.addCls('notcompleted');
        promoMechanicButton.setGlyph(0xf130);
        promoMechanicButton.isComplete = false;
    },

    setCompletedMechanicStep: function (isMars) {
        var promoMechanicButton = Ext.ComponentQuery.query('button[itemId=btn_promo_step3]')[0];
        if (isMars) promoMechanicButton.isMarsMechanicsComplete = true;
        else promoMechanicButton.isInstoreMechanicsComplete = true;
        if (promoMechanicButton.isInstoreMechanicsComplete && promoMechanicButton.isMarsMechanicsComplete) {
            promoMechanicButton.removeCls('notcompleted');
            promoMechanicButton.setGlyph(0xf133);
            promoMechanicButton.isComplete = true;
        }
    },

    getFullTextForMechanicButton: function (promoController) {
        var promoMechanic = Ext.ComponentQuery.query('promomechanic')[0],
            mechanicFields = promoController.getMechanicFields(promoMechanic);

        var marsText = this.getPartTextMarsMechanicButton(
            mechanicFields.marsMechanicFields.marsMechanicId,
            mechanicFields.marsMechanicFields.marsMechanicTypeId,
            mechanicFields.marsMechanicFields.marsMechanicDiscount,
            promoController.getMechanicListForUnlockDiscountField()
        );

        var instoreText = this.getPartTextMarsMechanicButton(
            mechanicFields.instoreMechanicFields.instoreMechanicId,
            mechanicFields.instoreMechanicFields.instoreMechanicTypeId,
            mechanicFields.instoreMechanicFields.instoreMechanicDiscount,
            promoController.getMechanicListForUnlockDiscountField()
        );

        return text = '<b>' + l10n.ns('tpm', 'promoStap').value('basicStep3') + '</b><br><p>Mars: ' + marsText + '<br>Instore Assumption: ' + instoreText + '</p>';
    },

    getPartTextMarsMechanicButton: function (mechanicId, mechanicTypeId, mechanicDiscount, mechanicListForUnlockDiscountField) {
        if (mechanicListForUnlockDiscountField.some(function (element) { return element === mechanicId.rawValue; })) {
            return mechanicId.rawValue + ' ' + mechanicDiscount.rawValue + '%';
        } else {
            return mechanicId.rawValue + ' ' + mechanicTypeId.rawValue;
        }
    },

    resetFields: function (fields) {
        fields.forEach(function (field) {
            field.reset();
        });
    },

    disableFields: function (fields) {
        fields.forEach(function (field) {
            field.setDisabled(true);
        });
    },

    // =============== MECHANIC END =============== 

    // хранит столбец плагина разворота строки
    columnExpander: null,
    // true - если изменили набор столбцов через "настроить таблицу"
    columnsUpdated: false,

    onPromoGridStoreLoad: function () {
        var grid = Ext.ComponentQuery.query('promo directorygrid')[0];
        /*     toolbar = grid.up('promo').down('custombigtoolbar');
         if (grid) {
             var selectionModel = grid.getSelectionModel();
             if (selectionModel.hasSelection()) {
                 var record = selectionModel.getSelection()[0];
                 //если производится расчет промо, кнопка Изменить должна быть недоступна для нажатия
                 if (record && record.data && record.data.Calculating) {
                     toolbar.down('#updatebutton').setDisabled(true);
                 } else {
                     toolbar.down('#updatebutton').setDisabled(false);
                 }
             }
         }*/
        // возвращаем плагин разворота строки, только если изменился набор столбцов
        if (this.columnExpander && grid) {
            var column = Ext.create('Ext.grid.column.Column', this.columnExpander);
            grid.headerCt.insert(0, column);
            this.columnExpander = null;
            grid.getView().refresh();
        }
    },

    onGridSettingsClick: function (button) {
        var grid = button.up('promo').down('grid');
        var columns = grid.headerCt.query('gridcolumn');

        // удаляем столбец плагина для разворачивания строки
        grid.headerCt.remove(columns[0]);
        this.columnExpander = columns[0];
        this.columnsUpdated = false;
        grid.getView().refresh();
    },

    // аккуратно, следующие методы для gridsettingswindow вызываются не только в промо, поэтому стоит условие
    onGridSettingsSaveClick: function () {
        var grid = Ext.ComponentQuery.query('promo directorygrid');
        this.columnsUpdated = grid.length > 0;
    },

    onGridSettingsWindowClose: function () {
        var grid = Ext.ComponentQuery.query('promo directorygrid')[0];

        // если набор полей не изменен возвращаем плагин здесь
        // иначе он вернется в методе onPromoGridStoreLoad
        if (grid && this.columnExpander && !this.columnsUpdated) {
            var column = Ext.create('Ext.grid.column.Column', this.columnExpander);
            grid.headerCt.insert(0, column);
            grid.getView().refresh();
            this.columnExpander = null;
        }
    },

    onPromoBudgetsAfterRender: function (panel) {
        var fieldsForSum = ['PlanPromoTIShopper', 'PlanPromoTIMarketing', 'PlanPromoBranding', 'PlanPromoBTL', 'PlanPromoCostProduction'];

        this.calculateShopperTI();
        this.calculateTotalCost('#promoBudgets_step1', 'PlanPromoCost', fieldsForSum);

        fieldsForSum = ['ActualPromoTIShopper', 'ActualPromoTIMarketing', 'ActualPromoBranding', 'ActualPromoBTL', 'ActualPromoCostProduction'];
        this.calculateTotalCost('#promoBudgets_step1', 'ActualPromoCost', fieldsForSum);

        /*var needRecountUplift = Ext.ComponentQuery.query('[name=NeedRecountUplift]')[0];
        var model = this.getRecord(panel.up('window'));
        var planUplift = Ext.ComponentQuery.query('[name=PlanPromoUpliftPercent]')[0];

        if (model) {
            if (model.data.NeedRecountUplift === true) {
                needRecountUplift.setValue(false);
            } else {
                needRecountUplift.setValue(true);
            }
        }

        if (needRecountUplift.disabled === true) {
            planUplift.setReadOnly(true);
            planUplift.addCls('readOnlyField');
        }

        if (Ext.ComponentQuery.query('#changePromo')[0].isVisible() === false) {
            needRecountUplift.setDisabled(false);

            if (needRecountUplift.value === true) {
                planUplift.setReadOnly(false);
                planUplift.removeCls('readOnlyField');
            }
        }*/


        // Установка стиля для readOnly полей.
        var fieldsForReadOnlyCls = [
            'ActualPromoTIShopper', 'PlanPromoTIShopper', 'PlanPromoTIMarketing', 'ActualPromoTIMarketing', 'PlanPromoCostProduction', 'ActualPromoCostProduction', 'PlanPromoCost', 'ActualPromoCost'
        ];

        this.setReadOnlyFields(fieldsForReadOnlyCls, false);

        // Блокировка кнопки Add Promo Support в режиме просмотра.
        var addSubItemButtons = panel.query('#addSubItem');
        addSubItemButtons.forEach(function (button) {
            button.setDisabled(panel.up('window').down('#changePromo').isVisible());
        })
    },

    setReadOnlyFields: function (fieldNames, all) {
        fieldNames.forEach(function (fieldName) {
            var fields = Ext.ComponentQuery.query('[name=' + fieldName + ']');
            for (var i = 0; i < fields.length; i++) {
                fields[i].addCls('readOnlyField');

                if (!all)
                    break;
            }
        });
    },

    setFieldsReadOnlyForSomeRole: function (promoeditorcustom) {
        var currentRole = App.UserInfo.getCurrentRole()['SystemName'];
        var fieldsWithCrud = Ext.ComponentQuery.query('[crudAccess*=' + currentRole + ']');

        // ------------------------ Client and Product -----------------------          
        var promoClientForm = promoeditorcustom.down('promoclient');
        var promoProductForm = promoeditorcustom.down('promobasicproducts');

        var clientCrudAccess = ['Administrator', 'FunctionalExpert', 'CMManager', 'CustomerMarketing', 'KeyAccountManager'];
        var productCrudAccess = ['Administrator', 'FunctionalExpert', 'CMManager', 'CustomerMarketing', 'KeyAccountManager'];

        if (clientCrudAccess.indexOf(currentRole) === -1) {
            promoClientForm.down('#choosePromoClientBtn').setDisabled(true);
        }

        if (productCrudAccess.indexOf(currentRole) === -1) {
            promoProductForm.setDisabledBtns(true);
        }

        // ---------------Other---------------  
        fieldsWithCrud.forEach(function (field) {
            field.removeCls('readOnlyField');
            if (field.disabled) {
                field.setDisabled(false);
            }
            if (field.readOnly) {
                field.setReadOnly(false);
            }
        });

        // Блокировка кнопок Add Promo Support для роли DemandPlanning.
        if (currentRole == 'DemandPlanning') {
            var addSubItemButtons = Ext.ComponentQuery.query('#addSubItem');
            if (addSubItemButtons.length > 0) {
                addSubItemButtons.forEach(function (button) {
                    button.disabled = true;
                    button.setDisabled = function () { return true; }
                });
            }
        }
    },

    onPromoActivityAfterRender: function (panel) {
        // Костыль для срабатывания события из PromoEditorCustomScroll.js
        var formStep2 = panel.down('#promoActivity_step2');
        formStep2.setHeight(600);

        var needRecountUplift = Ext.ComponentQuery.query('[itemId=PromoUpliftLockedUpdateCheckbox]')[0];
        var model = this.getRecord(panel.up('window'));
        var planUplift = Ext.ComponentQuery.query('[name=PlanPromoUpliftPercent]')[0];

        if (model) {
            if (model.data.NeedRecountUplift === true) {
                needRecountUplift.setValue(false);
            } else {
                needRecountUplift.setValue(true);
            }
        }

        if (needRecountUplift.disabled === true || Ext.ComponentQuery.query('#changePromo')[0].isVisible()) {
            planUplift.setReadOnly(true);
            planUplift.addCls('readOnlyField');
        }

        if (Ext.ComponentQuery.query('#changePromo')[0].isVisible() === false) {
            needRecountUplift.setDisabled(false);

            if (needRecountUplift.value === true) {
                planUplift.setReadOnly(false);
                planUplift.removeCls('readOnlyField');
            }
        }

        // Установка стиля для readOnly полей.
        var fieldsForReadOnlyCls = [
            'PlanPromoBaselineLSV', 'PlanPromoIncrementalLSV', 'PlanPromoLSV', 'PlanPromoPostPromoEffectLSV',
            'ActualPromoUpliftPercent', 'ActualPromoBaselineLSV', 'ActualPromoIncrementalLSV', 'ActualPromoLSV', 'ActualPromoLSVByCompensation', 'ActualPromoPostPromoEffectLSV',
        ];

        this.setReadOnlyFields(fieldsForReadOnlyCls, false);
    },

    // При изменении полей в Budgets Step 1 перерасчитываем Total Cost
    // А также меняем значения в Activity
    onPromoBudgetsStep1Change: function (field, newValue) {
        if (field.getName() != 'PlanPromoCost') {
            var fieldsForSum = ['PlanPromoTIShopper', 'PlanPromoTIMarketing', 'PlanPromoBranding', 'PlanPromoBTL', 'PlanPromoCostProduction'];
            this.calculateTotalCost('#promoBudgets_step1', 'PlanPromoCost', fieldsForSum);

            fieldsForSum = ['ActualPromoTIShopper', 'ActualPromoTIMarketing', 'ActualPromoBranding', 'ActualPromoBTL', 'ActualPromoCostProduction'];
            this.calculateTotalCost('#promoBudgets_step1', 'ActualPromoCost', fieldsForSum);
        }

        this.onCalculationPlanChange(field, newValue);
    },

    // Расчет Shopper TI в Promo Budgets Step 1
    calculateShopperTI: function () {
        /*var window = Ext.ComponentQuery.query('promoeditorcustom')[0];
        var promomechanic = window.down('promomechanic');
        var marsMechanicDiscount = promomechanic.down('numberfield[name=MarsMechanicDiscount]').getValue();
        var totalPromoLSV = window.down('#calculations_step2').down('numberfield[name=PlanPromoIncrementalLSV]').getValue();
        var shopperTIField = window.down('#calculations_step1').down('numberfield[name=PlanPromoTIShopper]');

        // если нет значения в одном из полей, то сбрасываем
        if (totalPromoLSV != null && marsMechanicDiscount != null)
            shopperTIField.setValue(totalPromoLSV * marsMechanicDiscount / 100.0);
        else
            shopperTIField.setValue(null);*/
    },

    // Расчет суммы:
    // panelStep  - id панели шага
    // totalField - name поля, куда пишется результат
    // sumFields  - name полей, значения которых суммируются
    calculateTotalCost: function (panelStep, totalField, sumFields, isPlan) {
        var panel = Ext.ComponentQuery.query('promoeditorcustom')[0].down(panelStep);
        var totalCostField = panel.down('numberfield[name=' + totalField + ']');
        var total = 0;

        for (var i = 0; i < sumFields.length; i++) {
            var val = panel.down('[name=' + sumFields[i] + ']').getValue();
            if (val !== null && val != undefined && val != '')
                total += parseFloat(val);
        }

        totalCostField.setValue(total);
    },

    onMarsMechanicDiscountChange: function (field) {
        this.calculateShopperTI();
    },

    onPlanPromoIncrementalLSVChange: function (field) {
        this.calculateShopperTI();
    },

    hideEditButtonForSomeRole: function () {
        var resource = 'Promoes';
        var allowedActions = [];

        // Получаем список точек достуа текущей роли и ресурса.
        var accessPointsForCurrentRoleAndResouce = App.UserInfo.getCurrentRole().AccessPoints.filter(function (point) {
            return point.Resource === resource;
        });

        // Сбор всех actions для текущей роли и ресурса.
        accessPointsForCurrentRoleAndResouce.forEach(function (point) {
            if (!allowedActions.some(function (action) { return action === point.Action })) {
                allowedActions.push(point.Action);
            }
        });

        if (!allowedActions.some(function (action) { return action === 'Post'; })) {
            Ext.ComponentQuery.query('#changePromo')[0].hide();
        }
    },

    // при изменении полей в Calculations меняем и в Promo Activity
    onCalculationPlanChange: function (field, newValue) {
        var panel = field.up('promoeditorcustom');
        var fieldActivity = panel.down('promoactivity').down('numberfield[name =' + field.getName() + ']');

        if (fieldActivity) {
            fieldActivity.setValue(newValue);
        }
    },

    onRejectButtonClick: function (button) {
        var window = button.up('window');
        var record = this.getRecord(window);

        this.showCommentWindow(record, window);
    },

    showCommentWindow: function (record, window) {
        var rejectreasonselectwindow = Ext.widget('rejectreasonselectwindow');
        rejectreasonselectwindow.record = record;
        rejectreasonselectwindow.promowindow = window;
        rejectreasonselectwindow.show();
    },

    onApplyActionButtonClick: function (button) {
        var windowReject = button.up('rejectreasonselectwindow');
        var commentField = windowReject.down('textarea[name=comment]');
        var rejectReasonField = windowReject.down('searchfield[name=RejectReasonId]');
        var me = this;

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
                            me.reFillPromoForm(windowPromo, newModel, directorygrid);
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

    getRecord: function (window) {
        var record = null;

        if (window.isCreating) {
            record = Ext.create('App.model.tpm.promo.Promo');
        } else if (window.assignedRecord != null) {
            record = window.assignedRecord;
        } else if (window.model != null) {
            record = window.model;
        } else {
            // Если запись обозначена в гриде - брать её
            var promoGrid = Ext.ComponentQuery.query('#promoGrid')[0];
            record = window.assignedRecord ? window.assignedRecord : promoGrid.down('directorygrid').getSelectionModel().getSelection()[0];
        }

        return record;
    },

    setReadOnlyProperty: function (readOnlyProperty, elements) {
        elements.forEach(function (el) {
            el.setReadOnly(readOnlyProperty);
            if (readOnlyProperty) {
                el.addClass('readOnlyField');
            } else {
                el.removeCls('readOnlyField');
            }
        });
    },

    onDeleteButtonClick: function (button) {
        var grid = this.getGridByButton(button),
            panel = grid.up('combineddirectorypanel'),
            selModel = grid.getSelectionModel();

        if (selModel.hasSelection()) {
            Ext.Msg.show({
                title: l10n.ns('core').value('deleteWindowTitle'),
                msg: l10n.ns('core').value('deleteConfirmMessage'),
                fn: onMsgBoxClose,
                scope: this,
                icon: Ext.Msg.QUESTION,
                buttons: Ext.Msg.YESNO,
                buttonText: {
                    yes: l10n.ns('core', 'buttons').value('delete'),
                    no: l10n.ns('core', 'buttons').value('cancel')
                }
            });
        } else {
            console.log('No selection');
        }

        function onMsgBoxClose(buttonId) {

            if (buttonId === 'yes') {
                var record = selModel.getSelection()[0],
                    store = grid.getStore(),
                    view = grid.getView(),
                    currentIndex = store.indexOf(record),
                    pageIndex = store.getPageFromRecordIndex(currentIndex),
                    endIndex = store.getTotalCount() - 2; // 2, т.к. после удаления станет на одну запись меньше

                currentIndex = Math.min(Math.max(currentIndex, 0), endIndex);
                panel.setLoading(l10n.ns('core').value('deletingText'));

                record.destroy({
                    scope: this,
                    success: function () {
                        selModel.deselectAll();
                        selModel.clearSelections();
                        store.data.removeAtKey(pageIndex);
                        store.totalCount--;

                        if (store.getTotalCount() > 0) {
                            view.bufferedRenderer.scrollTo(currentIndex, true, function () {
                                view.refresh();
                                panel.setLoading(false);
                                grid.setLoadMaskDisabled(true);
                                view.focusRow(currentIndex);
                            });
                        } else {
                            grid.setLoadMaskDisabled(true);
                            store.on({
                                single: true,
                                load: function (records, operation, success) {
                                    panel.setLoading(false);
                                    grid.setLoadMaskDisabled(false);
                                }
                            });
                            store.load();
                        }
                    },
                    failure: function () {
                        panel.setLoading(false);
                    }
                });
            }
        }
    },

    // =============== FILL PROMO EVENTS ===============
    // здесь описаны события загрузок различных частей карточки PROMO

    // количество загрузившихся элементов, всего 2
    countLoadedComponents: 0,

    // перезаполнить промо
    reFillPromoForm: function (window, record, grid) {
        this.countLoadedComponents = 0;
        var readOnly = window.readOnly ? window.readOnly : false;
        this.fillPromoForm(window, record, readOnly, false, grid);
    },

    // метод вызывать СТРОГО ОДИН РАЗ ЗА ОДНО ОТКРЫТИЕ PROMO
    // привязать события загрузок
    bindAllLoadEvents: function (window, record, isCopy) {
        this.bindEventMarsMechanicLoad(window, record);
        this.bindEventPlanInstoreMechanicLoad(window, record);
        this.bindEventActualInstoreMechanicLoad(window, record);
    },

    // если все компоненты загрузились, снимаем маску загрузки
    checkLoadingComponents: function () {
        if (this.countLoadedComponents < 0) {
            this.countLoadedComponents++;
        }

        if (this.countLoadedComponents == 0) {
            var window = Ext.ComponentQuery.query('promoeditorcustom');

            if (window.length > 0) {
                this.hideEditButtonForSomeRole();
                window[0].setLoading(false);

                this.initSignalR(window[0]);
            }
        }
    },

    bindEventMarsMechanicLoad: function (window, record) {
        var me = this;
        var mechanic = window.down('container[name=promo_step3]');

        var marsMechanicId = mechanic.down('searchcombobox[name=MarsMechanicId]');
        var marsMechanicTypeId = mechanic.down('searchcombobox[name=MarsMechanicTypeId]');
        var marsMechanicDiscount = mechanic.down('numberfield[name=MarsMechanicDiscount]');

        marsMechanicId.addListener('change', me.marsMechanicListener);
        marsMechanicTypeId.addListener('change', me.marsMechanicTypeListener);
        marsMechanicDiscount.addListener('change', me.marsMechanicDiscountListener);
    },

    bindEventPlanInstoreMechanicLoad: function (window, record) {
        var me = this;
        var mechanic = window.down('container[name=promo_step3]');

        var instoreMechanicId = mechanic.down('searchcombobox[name=PlanInstoreMechanicId]');
        var instoreMechanicTypeId = mechanic.down('searchcombobox[name=PlanInstoreMechanicTypeId]');
        var instoreMechanicDiscount = mechanic.down('numberfield[name=PlanInstoreMechanicDiscount]');

        instoreMechanicId.addListener('change', me.instoreMechanicListener);
        instoreMechanicTypeId.addListener('change', me.instoreMechanicTypeListener);
        instoreMechanicDiscount.addListener('change', me.instoreMechanicDiscountListener);
    },

    bindEventActualInstoreMechanicLoad: function (window, record) {
        var me = this;
        var mechanic = window.down('container[name=promoActivity_step1]');

        var actualInstoreMechanicId = mechanic.down('searchcombobox[name=ActualInstoreMechanicId]');
        var actualInstoreMechanicTypeId = mechanic.down('searchcombobox[name=ActualInstoreMechanicTypeId]');
        var actualInStoreDiscount = mechanic.down('numberfield[name=ActualInStoreDiscount]');

        actualInstoreMechanicId.addListener('change', me.actualMechanicListener);
        actualInstoreMechanicTypeId.addListener('change', me.actualMechanicTypeListener);
        actualInStoreDiscount.addListener('change', me.actualMechanicDiscountListener);
    },
    // =============== END FILL PROMO EVENTS ===============

    changeStatusPromo: function (promoId, statusId, window) {
        var params = 'id=' + promoId + '&promoNewStatusId=' + statusId;
        var grid = Ext.ComponentQuery.query('#promoGrid')[0];
        var me = this;

        window.setLoading(l10n.ns('core').value('savingText'));

        $.ajax({
            dataType: 'json',
            url: '/odata/Promoes/ChangeStatus?' + params,
            type: 'POST',
            success: function (data) {
                // привет EXT!
                App.model.tpm.promo.Promo.load(promoId, {
                    callback: function (newModel, operation) {
                        var directorygrid = grid ? grid.down('directorygrid') : null;

                        window.promoId = data.Id;
                        window.model = newModel;
                        me.reFillPromoForm(window, newModel, directorygrid);
                    }
                });

                // Если создание из календаря - обновляем календарь
                var scheduler = Ext.ComponentQuery.query('#nascheduler')[0];
                if (scheduler) {
                    scheduler.resourceStore.reload();
                    scheduler.eventStore.reload();
                }
            },
            error: function (data) {
                // при неудаче возвращаем старый статус
                if (window.previousStatusId) {
                    window.statusId = window.previousStatusId;
                }

                window.setLoading(false);
                App.Notify.pushError(data.statusText);
            }
        });
    },

    onPromoGridSelectionChange: function (selModel, selected) {
        this.onGridSelectionChange(selModel, selected);
    },

    onShowLogButtonClick: function (button) {
        //if (button.promoId) {
        this.onPrintPromoLog(button.up('promoeditorcustom'));
        //}
    },

    // открывает грид с promoproducts для промо
    onActivityUploadPromoProductsClick: function (button) {
        var promoProductWidget = Ext.widget('promoproduct');
        var promoForm = button.up('promoeditorcustom');
        var record = this.getRecord(promoForm);
        var me = this;

        if (promoProductWidget) {
            promoProductWidget.addListener('afterrender', function () {
                var toolbar = promoProductWidget.down('custombigtoolbar');
                var importBtn = promoProductWidget.down('[action=FullImportXLSX]');

                toolbar.down('#createbutton').hide();
                importBtn.action += '?promoId=' + record.get('Id');
            })

            // загружать нужно только для текущего промо
            var store = promoProductWidget.down('grid').getStore();
            store.setFixedFilter('PromoIdFilter', {
                property: 'PromoId',
                operation: 'Equals',
                value: record.get('Id')
            })

            Ext.widget('selectorwindow', {
                title: '',
                itemId: Ext.String.format('{0}_{1}_{2}', 'activityUploadPromoProducts', promoProductWidget.getXType(), 'selectorwindow'),
                items: [promoProductWidget],
                buttons: [{
                    text: 'Close',
                    style: { "background-color": "#3F6895" },
                    listeners: {
                        click: function (button) {
                            button.up('selectorwindow').close();
                        }
                    }
                }],
                listeners: {
                    beforeclose: function () {
                        // проверяем идут ли расчеты
                        App.model.tpm.promo.Promo.load(record.get('Id'), {
                            callback: function (newModel, operation) {
                                if (newModel.data.Calculating) {
                                    var grid = Ext.ComponentQuery.query('#promoGrid')[0];
                                    var directorygrid = grid ? grid.down('directorygrid') : null;

                                    promoForm.model = newModel;
                                    me.reFillPromoForm(promoForm, newModel, directorygrid);

                                    // Если создание из календаря - обновляем календарь
                                    var scheduler = Ext.ComponentQuery.query('#nascheduler')[0];
                                    if (scheduler) {
                                        scheduler.resourceStore.reload();
                                        scheduler.eventStore.reload();
                                    }
                                }
                            }
                        });
                    }
                }
            }).show();
        }
    },

    //окно логов
    onPrintPromoLog: function (window, grid, close) {
        //var printPromoCalculatingWin = Ext.create('App.view.tpm.promo.PromoCalculatingWindow');
        //printPromoCalculatingWin.show();
        
        var calculatingInfoWindow = Ext.create('App.view.tpm.promocalculating.CalculatingInfoWindow');
        calculatingInfoWindow.on({
            beforeclose: function () {
                if ($.connection.logHub)
                    $.connection.logHub.server.unsubscribeLog();
            }
        });

        calculatingInfoWindow.show();
        $.connection.logHub.server.subscribeLog();
    },

    createTaskCheckCalculation: function (window) {
        //var record = this.getRecord(window);
        //var me = this;

        //record.set('Calculating', true);

        //var taskForLog = {
        //    run: function () {
        //        // Периодическое чтение лога в окно и проверка на окончание
        //        var params = 'promoId=' + record.get('Id');
        //        Ext.Ajax.request({
        //            method: 'POST',
        //            url: 'odata/Promoes/ReadPromoCalculatingLog?' + params,
        //            success: function (response, opts) {
        //                var result = Ext.JSON.decode(response.responseText);

        //                // если промо закрыли, нужно прекратить запросы
        //                if (!window.isVisible()) {
        //                    Ext.TaskManager.stop(taskForLog);
        //                } else {
        //                    var logWindow = Ext.ComponentQuery.query('promocalculatingwindow');

        //                    me.setCalculatingInformation(result);

        //                    //Проверка на закрытие
        //                    if (result.code == -1) { // ошибка во время проверки
        //                        Ext.TaskManager.stop(taskForLog);
        //                        Ext.Msg.alert("Внимание", result.opData);
        //                        me.unBlockPromo(window);
        //                    } else if (result.code == 1) { // задача завершилась
        //                        Ext.TaskManager.stop(taskForLog);
        //                        logWindow.jobEnded = true;
        //                        me.unBlockPromo(window);
        //                    }
        //                }
        //            },
        //            failure: function () {
        //                Ext.Msg.alert('ReadPromoCalculatingLog error');
        //            }
        //        });
        //    },
        //    interval: (1000) * 0.5 // 0.5 секунды
        //};

        //Ext.TaskManager.start(taskForLog);
    },

    setCalculatingInformation: function (result) {
        var infoWindow = Ext.ComponentQuery.query('calculatinginfowindow');

        if (infoWindow.length > 0) {
            infoWindow = infoWindow[0];
            var infoGrid = infoWindow.down('grid'),
                gridInfoToolbar = infoGrid.down('gridinfotoolbar'),
                infoStore = infoGrid.getStore(),
                descriptionField = infoWindow.down('triggerfield[name=Task]'),
                statusField = infoWindow.down('triggerfield[name=Status]');

            if (result.status === 'INPROGRESS') {
                this.changeCls(statusField, 'readOnlyField', 'inprogressField');
                this.changeCls(statusField.triggerEl, '', 'inprogressField-trigger');
                this.changeCls(statusField.triggerCell, '', 'inprogressField-trigger-cell');
            } else if (result.status === 'COMPLETE') {
                this.changeCls(statusField, 'inprogressField', 'completeField');
                this.changeCls(statusField.triggerEl, 'inprogressField-trigger', 'completeField-trigger');
                this.changeCls(statusField.triggerCell, 'inprogressField-trigger-cell', 'completeField-trigger-cell');
            } else if (result.status === 'PARTIAL COMPLETE') {
                this.changeCls(statusField, 'inprogressField', 'completeField');
                this.changeCls(statusField.triggerEl, 'inprogressField-trigger', 'completeField-trigger');
                this.changeCls(statusField.triggerCell, 'inprogressField-trigger-cell', 'completeField-trigger-cell');
            } else if (result.status === 'ERROR') {
                if (statusField.hasCls('readOnlyField')) {
                    this.changeCls(statusField, 'readOnlyField', 'errorField');
                    this.changeCls(statusField.triggerEl, '', 'errorField-trigger');
                    this.changeCls(statusField.triggerCell, '', 'errorField-trigger-cell');
                }
                else {
                    this.changeCls(statusField, 'inprogressField', 'errorField');
                    this.changeCls(statusField.triggerEl, 'inprogressField-trigger', 'errorField-trigger');
                    this.changeCls(statusField.triggerCell, 'inprogressField-trigger-cell', 'errorField-trigger-cell');
                }
            }

            descriptionField.setValue(result.description);
            statusField.setValue(result.status);
            // TODO: добавить ERROR

            var logData = [];
            var logText = result.data;
            if (logText) {
                logData = this.parseLogData(logText, infoWindow);
                infoWindow.logText = logText;
            }

            if (logData.length > 0) {
                infoStore.loadRecords(logData);
                this.colorRaws(infoGrid);

                var displayItem = gridInfoToolbar.child('#displayItem'),
                    msg = Ext.String.format(l10n.ns('core', 'gridInfoToolbar').value('gridInfoMsg'), infoStore.data.length);

                if (displayItem) {
                    displayItem.setText(msg);
                }
            }

            // если пользователь вызвал окно лога со всеми сообщениями, то обновляем и его
            var oldLogWindow = Ext.ComponentQuery.query('promocalculatingwindow');
            if (oldLogWindow.length > 0) {
                var textareaOldLogWindow = oldLogWindow[0].down('textarea');

                if (textareaOldLogWindow) {
                    textareaOldLogWindow.setValue(infoWindow.logText);
                }
            }
        }
    },

    changeCls: function (el, prevCls, nextCls) {
        el.removeCls(prevCls);
        el.addCls(nextCls);
    },

    parseLogData: function (text, window) {
        var data = [],
            startTag = '[',
            endTag = ']:',
            message = '',
            errorType = 'ERROR',
            warningType = 'WARNING',
            infoType = 'INFO',
            errorLabel = window.down('button[name=logError]'),
            warningLabel = window.down('button[name=logWarning]'),
            messageLabel = window.down('button[name=logMessage]'),
            errorCount = 0,
            warningCount = 0,
            messageCount = 0;

        // ищем по два тега и текст между ними и будет сообщением
        var findedTagFirst = this.findTagMessage(0, text, startTag, endTag, [errorType, warningType, infoType]);

        do {            
            var findedTagSecond = this.findTagMessage(findedTagFirst.indexEnd, text, startTag, endTag, [errorType, warningType, infoType]);

            if (findedTagFirst.tag != null) {
                if (findedTagFirst.tag === errorType) {
                    errorCount++;
                } else if (findedTagFirst.tag === warningType) {
                    warningCount++;
                } else if (findedTagFirst.tag === infoType) {
                    messageCount++;
                }

                message = findedTagSecond.tag != null ? text.substring(findedTagFirst.indexEnd + endTag.length, findedTagSecond.indexStart) :
                                                        text.substring(findedTagFirst.indexEnd + endTag.length);

                data.push(Ext.create('App.model.tpm.promocalculating.CalculatingInfoLog', {
                    Type: findedTagFirst.tag,
                    Message: message
                }));

                findedTagFirst = findedTagSecond;
            }

        } while (findedTagFirst.tag != null);

        errorLabel.setText(errorCount + ' Errors');
        warningLabel.setText(warningCount + ' Warnings');
        messageLabel.setText(messageCount + ' Infos');

        return data;
    },

    // ищет тег в сообщении
    findTagMessage: function (startIndex, text, startTag, endTag, typesTag) {
        var indexStart = startIndex;        
        var indexEnd = 0;
        var tag = null;

        do {
            indexStart = text.indexOf(startTag, indexStart);
            indexEnd = text.indexOf(endTag, indexStart);

            if (indexStart !== -1) {
                var indexTag = typesTag.indexOf(text.substring(indexStart++ + startTag.length, indexEnd));                
                tag = indexTag !== -1 ? typesTag[indexTag] : null;
            }

        } while (tag == null && indexStart !== -1);

        return { indexStart: indexStart - 1, indexEnd: indexEnd, tag: tag };
    },

    blockPromo: function (window, record) {
        var rec = record || this.getRecord(window);
        var toolbar = window.down('customtoptoolbar');

        rec.set('Calculating', true);

        toolbar.items.items.forEach(function (item, i, arr) {
            item.el.setStyle('backgroundColor', '#B53333');
            if (item.xtype == 'button' && ['btn_publish', 'btn_undoPublish', 'btn_sendForApproval', 'btn_reject', 'btn_approve', 'btn_cancel', 'btn_plan', 'btn_close', 'btn_backToFinished'].indexOf(item.itemId) > -1) {
                item.setDisabled(true);
            }
        });

        toolbar.el.setStyle('backgroundColor', '#B53333');

        var label = toolbar.down('label[name=promoName]');
        if (label) {
            label.setText(label.text + ' — Promo is blocked for recalculations');
        }

        window.down('#changePromo').setDisabled(true);
        toolbar.down('#btn_showlog').addCls('showlog');
        //toolbar.down('#btn_showlog').show();
        toolbar.down('#btn_showlog').promoId = rec.data.Id;
    },

    unBlockPromo: function (window) {
        var record = this.getRecord(window);
        var toolbar = window.down('customtoptoolbar');
        var me = this;

        if (toolbar) {
            toolbar.items.items.forEach(function (item, i, arr) {
                item.el.setStyle('backgroundColor', '#3f6895');
                if (item.xtype == 'button' && ['btn_publish', 'btn_undoPublish',
                    'btn_sendForApproval', 'btn_reject', 'btn_approve', 'btn_cancel', 'btn_plan', 'btn_close', 'btn_backToFinished'].indexOf(item.itemId) > -1) {
                    item.setDisabled(false);
                }
            });

            toolbar.el.setStyle('backgroundColor', '#3f6895');

            var label = toolbar.down('label[name=promoName]');
            if (label && label.text.lastIndexOf(' — ') !== -1) {
                var text = label.text.substr(0, label.text.lastIndexOf(' — '));
                label.setText(text);
            }

            window.down('#savePromo').setDisabled(false);
            window.down('#saveAndClosePromo').setDisabled(false);
            window.down('#changePromo').setDisabled(false);

            if (this.detailButton) {
                window.down('#closePromo').setVisible(true);
                window.down('#cancelPromo').setVisible(false);
            }
            else {
                window.down('#cancelPromo').setVisible(true);
                window.down('#closePromo').setVisible(false);
            }

            //toolbar.down('#btn_showlog').hide();

            if (App.UserInfo.getCurrentRole().SystemName.toLowerCase() == 'administrator' && record.data.PromoStatusSystemName != 'Draft')
                window.down('#btn_recalculatePromo').show();
        }

        // если расчеты закончились, необходимо обновить форму
        window.setLoading(l10n.ns('core').value('savingText'));

        App.model.tpm.promo.Promo.load(record.get('Id'), {
            callback: function (newModel, operation, success) {
                if (success) {
                    var grid = Ext.ComponentQuery.query('#promoGrid')[0];
                    var directorygrid = grid ? grid.down('directorygrid') : null;
                    var currentRecord = me.getRecord(window);

                    if (currentRecord.get('Calculating')) {
                        window.model = newModel;
                        me.reFillPromoForm(window, newModel, directorygrid);

                        // Если создание из календаря - обновляем календарь
                        var scheduler = Ext.ComponentQuery.query('#nascheduler')[0];
                        if (scheduler) {
                            scheduler.resourceStore.reload();
                            scheduler.eventStore.reload();
                        }
                    }
                }
                else {
                    window.setLoading(false);
                    App.Notify.pushError('Error!');
                }
            }
        });
    },

    colorRaws: function (grid) {
        grid.getView().getRowClass = function (record, rowIndex, rowParams, store) {
            if (record.data.Type === 'WARNING') {
                return 'warningraw'
            } else if (record.data.Type === 'ERROR') {
                return 'errorraw'
            } else {
                return ''
            }
        }
    },

    // т.к. Форма редактирования кастомная, нет необходимости её обновлять
    updateDetailForm: function () { },

    // Показывать промо для которых возможно изменение статуса под текущей ролью
    onShowEditableButtonClick: function (button) {
        var store = this.getGridByButton(button).getStore(),
            proxy = store.getProxy()
        proxy.extraParams.canChangeStateOnly = !proxy.extraParams.canChangeStateOnly;

        store.clearData();
        store.load();

        if (proxy.extraParams.canChangeStateOnly) {
            button.addCls('showEditablePromo-btn-active');
        } else {
            button.removeCls('showEditablePromo-btn-active');
        }
    },

    onPromoBudgetsDetailsWindowAfterRender: function (window) {
        var promoController = App.app.getController('tpm.promo.Promo');
        var record = promoController.getRecord(Ext.ComponentQuery.query('promoeditorcustom')[0]);

        // Plan - Total Cost Budgets
        window.down('[name=PlanPromoCost]').setValue(record.data.PlanPromoCost);
        window.down('[name=PlanPromoTIMarketing]').setValue(record.data.PlanPromoTIMarketing);
        window.down('[name=PlanPromoCostProduction]').setValue(record.data.PlanPromoCostProduction);
        window.down('[name=PlanPromoTIShopper]').setValue(record.data.PlanPromoTIShopper);
        window.down('[name=PlanPromoBranding]').setValue(record.data.PlanPromoBranding);
        window.down('[name=PlanPromoBTL]').setValue(record.data.PlanPromoBTL);

        // Plan - Marketing TI
        window.down('[name=budgetDet-PlanX-sites]').setValue(record.data.PlanPromoXSites);
        window.down('[name=budgetDet-PlanCatalog]').setValue(record.data.PlanPromoCatalogue);
        window.down('[name=budgetDet-PlanPOSM]').setValue(record.data.PlanPromoPOSMInClient);

        // Plan - Cost Production
        window.down('[name=budgetDet-PlanCostProdX-sites]').setValue(record.data.PlanPromoCostProdXSites);
        window.down('[name=budgetDet-PlanCostProdCatalog]').setValue(record.data.PlanPromoCostProdCatalogue);
        window.down('[name=budgetDet-PlanCostProdPOSM]').setValue(record.data.PlanPromoCostProdPOSMInClient);

        // Actual - Total Cost Budgets
        window.down('[name=ActualPromoCost]').setValue(record.data.ActualPromoCost);
        window.down('[name=ActualPromoTIMarketing]').setValue(record.data.ActualPromoTIMarketing);
        window.down('[name=ActualPromoCostProduction]').setValue(record.data.ActualPromoCostProduction);
        window.down('[name=ActualPromoTIShopper]').setValue(record.data.ActualPromoTIShopper);
        window.down('[name=ActualPromoBranding]').setValue(record.data.ActualPromoBranding);
        window.down('[name=ActualPromoBTL]').setValue(record.data.ActualPromoBTL);

        // Actual - Marketing TI
        window.down('[name=budgetDet-ActualX-sites]').setValue(record.data.ActualPromoXSites);
        window.down('[name=budgetDet-ActualCatalog]').setValue(record.data.ActualPromoCatalogue);
        window.down('[name=budgetDet-ActualPOSM]').setValue(record.data.ActualPromoPOSMInClient);

        // Actual - Cost Production
        window.down('[name=budgetDet-ActualCostProdX-sites]').setValue(record.data.ActualPromoCostProdXSites);
        window.down('[name=budgetDet-ActualCostProdCatalog]').setValue(record.data.ActualPromoCostProdCatalogue);
        window.down('[name=budgetDet-ActualCostProdPOSM]').setValue(record.data.ActualPromoCostProdPOSMInClient);
    },

    onPromoFinanceDetailsWindowAfterRender: function (window) {
        var promoController = App.app.getController('tpm.promo.Promo'),
            record = promoController.getRecord(Ext.ComponentQuery.query('promoeditorcustom')[0]);

        window.down('[name=PlanPromoNetIncrementalLSV]').setValue(record.data.PlanPromoNetIncrementalLSV);
        window.down('[name=PlanPromoNetLSV]').setValue(record.data.PlanPromoNetLSV);
        window.down('[name=PlanPromoIncrementalNSV]').setValue(record.data.PlanPromoIncrementalNSV);
        window.down('[name=PlanPromoNetIncrementalNSV]').setValue(record.data.PlanPromoNetIncrementalNSV);
        window.down('[name=PlanPromoPostPromoEffectLSV]').setValue(record.data.PlanPromoPostPromoEffectLSV);
        window.down('[name=PlanPromoNetNSV]').setValue(record.data.PlanPromoNetNSV);
        window.down('[name=PlanPromoIncrementalMAC]').setValue(record.data.PlanPromoIncrementalMAC);
        window.down('[name=PlanPromoNetIncrementalMAC]').setValue(record.data.PlanPromoNetIncrementalMAC);
        window.down('[name=PlanPromoIncrementalEarnings]').setValue(record.data.PlanPromoIncrementalEarnings);
        window.down('[name=PlanPromoNetIncrementalEarnings]').setValue(record.data.PlanPromoNetIncrementalEarnings);
        window.down('[name=PlanPromoROIPercent]').setValue(record.data.PlanPromoROIPercent);
        window.down('[name=PlanPromoNetROIPercent]').setValue(record.data.PlanPromoNetROIPercent);

        window.down('[name=ActualPromoNetIncrementalLSV]').setValue(record.data.ActualPromoNetIncrementalLSV);
        window.down('[name=ActualPromoNetLSV]').setValue(record.data.ActualPromoNetLSV);
        window.down('[name=ActualPromoIncrementalNSV]').setValue(record.data.ActualPromoIncrementalNSV);
        window.down('[name=ActualPromoNetIncrementalNSV]').setValue(record.data.ActualPromoNetIncrementalNSV);
        window.down('[name=ActualProductPostPromoEffectLSV]').setValue(record.data.ActualPromoPostPromoEffectLSV);
        window.down('[name=ActualPromoNetNSV]').setValue(record.data.ActualPromoNetNSV);
        window.down('[name=ActualPromoIncrementalMAC]').setValue(record.data.ActualPromoIncrementalMAC);
        window.down('[name=ActualPromoNetIncrementalMAC]').setValue(record.data.ActualPromoNetIncrementalMAC);
        window.down('[name=ActualPromoIncrementalEarnings]').setValue(record.data.ActualPromoIncrementalEarnings);
        window.down('[name=ActualPromoNetIncrementalEarnings]').setValue(record.data.ActualPromoNetIncrementalEarnings);
        window.down('[name=ActualPromoROIPercent]').setValue(record.data.ActualPromoROIPercent);
        window.down('[name=ActualPromoNetROIPercent]').setValue(record.data.ActualPromoNetROIPercent);
    },

    onPromoActivityDetailsWindowAfterRender: function (window) {
        var promoController = App.app.getController('tpm.promo.Promo');
        var record = promoController.getRecord(Ext.ComponentQuery.query('promoeditorcustom')[0]);

        var actualInStoreMechanicId = window.down('[name=ActualInstoreMechanicId]');
        var actualInStoreMechanicTypeId = window.down('[name=ActualInstoreMechanicTypeId]');

        // Instore Assumption
        actualInStoreMechanicId.setValue(new App.model.tpm.mechanic.Mechanic({
            Id: record.data.ActualInStoreMechanicId,
            Name: record.data.ActualInStoreMechanicName
        }));

        actualInStoreMechanicTypeId.setValue(new App.model.tpm.mechanictype.MechanicType({
            Id: record.data.ActualInStoreMechanicTypeId,
            Name: record.data.PlanInstoreMechanicTypeName
        }));

        window.down('[name=ActualInStoreDiscount]').setValue(record.data.ActualInStoreDiscount == 0 ? null : record.data.ActualInStoreDiscount);

        // Plan - Activity
        window.down('[name=PlanPromoUpliftPercent]').setValue(record.data.PlanPromoUpliftPercent);
        window.down('[name=PlanPromoBaselineLSV]').setValue(record.data.PlanPromoBaselineLSV);
        window.down('[name=PlanPromoIncrementalLSV]').setValue(record.data.PlanPromoIncrementalLSV);
        window.down('[name=PlanPromoLSV]').setValue(record.data.PlanPromoLSV);
        window.down('[name=PlanPromoPostPromoEffectLSV]').setValue(record.data.PlanPromoPostPromoEffectLSV);

        // In Store Shelf Price
        window.down('[name=ActualInStoreShelfPrice]').setValue(record.data.ActualInStoreShelfPrice);
        window.down('[name=PlanInStoreShelfPrice]').setValue(record.data.PlanInStoreShelfPrice);

        // Actual - Activity
        window.down('[name=InvoiceNumber]').setValue(record.data.InvoiceNumber);
        window.down('[name=ActualPromoUpliftPercent]').setValue(record.data.ActualPromoUpliftPercent);
        window.down('[name=ActualPromoBaselineLSV]').setValue(record.data.PlanPromoBaselineLSV);
        window.down('[name=ActualPromoIncrementalLSV]').setValue(record.data.ActualPromoIncrementalLSV);
        window.down('[name=ActualPromoLSVByCompensation]').setValue(record.data.ActualPromoLSVByCompensation);
        window.down('[name=ActualPromoLSV]').setValue(record.data.ActualPromoLSV);
        window.down('[name=ActualPromoPostPromoEffectLSV]').setValue(record.data.ActualPromoPostPromoEffectLSV);
    },

    onPromoProductSubrangeDetailsWindowAfterRender: function (window) {
        var record = this.getRecord(window);        
        var productTreeGrid = Ext.widget('producttreegrid', {
            disabled: true,
            cls: 'template-tree scrollpanel promo-product-window',
            store: {
                model: 'App.model.tpm.producttree.ProductTree',
                autoLoad: false,
                root: {},
                storeId: 'TempProducTree'
            },
        });

        var store = productTreeGrid.getStore();
        // даже для разных сторов proxy один
        var proxy = store.getProxy();
        var oldExtraParams = {
            dateFilter: proxy.extraParams.dateFilter,
            filterParameter: proxy.extraParams.filterParameter,
            productTreeObjectIds: proxy.extraParams.productTreeObjectIds,
            promoId: proxy.extraParams.promoId,
            view: proxy.extraParams.view
        };

        store.on({
            load: {
                fn: function () {
                    var productTreeController = App.app.getController('tpm.product.ProductTree');
                    var trueRoot = store.getById('1000000');

                    store.setRootNode(trueRoot);
                    productTreeController.setCheckTree(store.getRootNode().childNodes, true);
                    store.getProxy().extraParams = oldExtraParams; // возвращаем параметры в исходную
                    Ext.ComponentQuery.query('#promo_step2')[0].down('producttreegrid').unmask();
                },
                single: true
            }
        });

        proxy.extraParams.dateFilter = null;
        proxy.extraParams.promoId = record.get('Id');
        proxy.extraParams.view = true;
        store.load();

        window.down('#containerForProductTreeGrid').add(productTreeGrid);
    },
    // сбрасываем параметр показывающий только промо зависящие от текущего пользователя при закрытии панели
    onPromoGridBeforeDestroy: function (panel) {
        var store = panel.down('directorygrid').getStore(),
            proxy = store.getProxy();
        proxy.extraParams.canChangeStateOnly = false;
        return true;
    },

    //фунция для определения положения ползунка, чтобы после закрытия окна промо и перезагрузки стора прокрутить на прежнее место
    getScrollPosition: function (button) {
        if (button) {
            var grid = button.up('combineddirectorypanel').down('directorygrid'),
                view = grid.getView(),
                positionY = $(view.getEl().dom).data('jsp').getContentPositionY();

            view.positionY = positionY;
        }
    },

    initSignalR: function (window) {
        var record = this.getRecord(window);
        var promoId = record.get('Id');
        var blocked = record.get('Calculating');

        $.connection.logHub.server.subscribeStatus(promoId, blocked);
        window.down('#btn_showlog').setDisabled(false);
    },

    recalculatePromo: function (btn) {
        var window = btn.up('promoeditorcustom');
        var record = this.getRecord(window);
        var promoId = record.get('Id');
        var me = this;

        window.setLoading(l10n.ns('core').value('savingText'));

        $.ajax({
            dataType: 'json',
            url: '/odata/Promoes/RecalculatePromo?promoId=' + promoId,
            type: 'POST',
            success: function (response) {
                var data = Ext.JSON.decode(response.value);

                if (data.success) {
                    // привет EXT!
                    App.model.tpm.promo.Promo.load(promoId, {
                        callback: function (newModel, operation) {
                            var grid = Ext.ComponentQuery.query('#promoGrid')[0];
                            var directorygrid = grid ? grid.down('directorygrid') : null;

                            window.promoId = data.Id;
                            window.model = newModel;
                            me.reFillPromoForm(window, newModel, directorygrid);
                            //24.06.19 Лог не показываем
                            //if (newModel.get('Calculating'))
                            //    me.onPrintPromoLog(window, grid, close);
                        }
                    });
                }
                else {
                    window.setLoading(false);
                    App.Notify.pushError(l10n.ns('tpm', 'text').value('failedLoadData'));
                }

            },
            error: function (data) {
                window.setLoading(false);
                App.Notify.pushError(data.responseJSON["odata.error"].innererror.message);
            }
        });
    },

    // --------------------- Выбор клиента -----------------//
    // поспотреть параметры клиента
    onPromoClientSettignsClick: function (button) {
        var promoClientForm = button.up('promoclient');

        promoClientForm.showSettings();
    },

    // событие нажатия кнопки выбора клиента
    onChoosePromoClientClick: function (button) {
        var promoClientForm = button.up('promoclient');
        var me = this;

        // передаем также call-back функцию, отвечающую за dispatch
        // также устанавливаем подписи кнопок (Promo Basic Steps)
        promoClientForm.chooseClient(function (clientTreeRecord) {
            var promoeditorcustom = promoClientForm.up('promoeditorcustom');
            var clientTreeRecordRaw = clientTreeRecord ? clientTreeRecord.raw : null;

            me.checkParametersAfterChangeClient(clientTreeRecordRaw, promoeditorcustom);
        });
    },

    // обработчик выбора клиента в форме выбора клиента
    onClientTreeCheckChange: function (item, checked, eOpts) {
        var treegrid = item.store.ownerTree;
        var nodes = treegrid.getRootNode().childNodes;
        var clientTree = treegrid.up('clienttree');
        var editorForm = clientTree.down('editorform');
        var clientTreeController = this.getController('tpm.client.ClientTree');

        clientTreeController.setCheckTree(nodes, false);

        if (checked) {
            item.set('checked', true);
            clientTree.choosenClientObjectId = item.get('ObjectId');
            clientTreeController.updateTreeDetail(editorForm, item);

            var store = item.store;
            var parent = store.getNodeById(item.get('parentId'));

            if (parent) {
                while (parent.data.root !== true) {
                    parent.set('checked', true);
                    parent = store.getNodeById(parent.get('parentId'));
                }
            }
        }
        else {
            clientTree.choosenClientObjectId = null;
        }
    },

    // при нажатии кнопки выбрать в форме выбора клиента
    onChooseClientTreeClick: function (button) {
        var promoClientChooseWnd = button.up('promoclientchoosewindow');
        var clientTree = promoClientChooseWnd.down('clienttree');

        // если сменился клиент и есть call-back функция, то вызываем её, возвращая выбранного клиента
        if (promoClientChooseWnd.choosenClientObjectId != clientTree.choosenClientObjectId && promoClientChooseWnd.callBackChooseFnc) {
            var clientTreeGrid = clientTree.down('clienttreegrid');
            var clientTreeStore = clientTreeGrid.getStore();
            var clientTreeRecord = clientTreeStore.getById(clientTree.choosenClientObjectId);

            promoClientChooseWnd.callBackChooseFnc(clientTreeRecord);            
        }

        promoClientChooseWnd.close();
    },

    // устанавливаем информацию на кнопке Step1 Basic Promo
    // а также меняем клиента в записи окна
    setInfoPromoBasicStep1: function (promoClientForm) {
        var promoWindow = Ext.ComponentQuery.query('promoeditorcustom')[0];
        var clientButton = promoWindow.down('button#btn_promo_step1');

        // если клиент выбран, то clientTreeRecord != null
        if (promoClientForm.clientTreeRecord) {
            var text = '<b>' + l10n.ns('tpm', 'promoStap').value('basicStep1') + '</b><br><p>';
            var displayHierarchy = promoClientForm.clientTreeRecord.FullPathName.replace(/>/g, '<span style="font-size: 13px; font-family: MaterialDesignIcons; color: #A7AFB7"> </span>');

            promoWindow.clientHierarchy = promoClientForm.clientTreeRecord.FullPathName;
            promoWindow.clientTreeId = promoClientForm.clientTreeRecord.ObjectId;
            promoWindow.clientTreeKeyId = promoClientForm.clientTreeRecord.Id;
            clientButton.setText(text + displayHierarchy + '</p>');
            clientButton.removeCls('notcompleted');
            clientButton.setGlyph(0xf133);
            clientButton.isComplete = true;
        }
        else {
            promoWindow.clientHierarchy = null;
            promoWindow.clientTreeId = null;
            promoWindow.clientTreeKeyId = null;
            clientButton.setText('<b>' + l10n.ns('tpm', 'promoStap').value('basicStep1') + '</b>');
            clientButton.addCls('notcompleted');
            clientButton.setGlyph(0xf130);
            clientButton.isComplete = false;

            // dispatch
            var promoperiod = promoWindow.down('promoperiod');
            var periodButton = promoWindow.down('button#btn_promo_step4');
            var dispatchStartDate = promoperiod.down('[name=DispatchStartDate]');
            var dispatchEndDate = promoperiod.down('[name=DispatchEndDate]');
            var durationStartDate = promoperiod.down('[name=DurationStartDate]');
            var durationEndDate = promoperiod.down('[name=DurationEndDate]');

            dispatchStartDate.reset();
            dispatchEndDate.reset();

            var durationPeriod = durationStartDate.getValue() && durationEndDate.getValue() ? 'c ' + Ext.Date.format(durationStartDate.getValue(), "d.m.Y") + ' по ' + Ext.Date.format(durationEndDate.getValue(), "d.m.Y") : '';
            var text = '<b>' + l10n.ns('tpm', 'promoStap').value('basicStep4') + '</b><br><p>Promo: ' + durationPeriod + '<br>Dispatch: ' + '</p>';

            var fieldSetDispatch = dispatchStartDate.up('fieldset');
            fieldSetDispatch.setTitle('Dispatch (0 days)');

            periodButton.setText(text);
            periodButton.addCls('notcompleted');
            periodButton.setGlyph(0xf130);
        }

        // проверяем готовность Promo Basic
        checkMainTab(clientButton.up(), promoWindow.down('button[itemId=btn_promo]'));
    },

    // следующие две функции старые, управляют dispatch
    checkParametersAfterChangeClient: function (clientTreeRecord, promoeditorcustom) {
        var me = this;
        var promoClientForm = promoeditorcustom.down('promoclient');
        promoClientForm.fillForm(clientTreeRecord);

        var periodButton = promoeditorcustom.down('button#btn_promo_step4');
        var promoperiod = promoeditorcustom.down('promoperiod');
        var dispatchStartDate = promoperiod.down('[name=DispatchStartDate]');
        var dispatchEndDate = promoperiod.down('[name=DispatchEndDate]');
        var durationStartDate = promoeditorcustom.down('[name=DurationStartDate]');
        var durationEndDate = promoeditorcustom.down('[name=DurationEndDate]');
        var calculation = Ext.ComponentQuery.query('promocalculation')[0];
        var activity = Ext.ComponentQuery.query('promoactivity')[0];

        if (calculation && activity) {
            var planPostPromoEffectFromCalculation = calculation.down('trigger[name=PlanPostPromoEffect]');
            var changePromoButton = promoeditorcustom.down('#changePromo');
            var model = this.getRecord(promoeditorcustom);

            model.data.PlanPostPromoEffectW1 = clientTreeRecord.PostPromoEffectW1 || 0;
            model.data.PlanPostPromoEffectW2 = clientTreeRecord.PostPromoEffectW2 || 0;
            model.data.PlanPostPromoEffect = (clientTreeRecord.PostPromoEffectW1 + clientTreeRecord.PostPromoEffectW2) || 0;

            if (changePromoButton) {
                if (changePromoButton.hidden === true) {
                    planPostPromoEffectFromCalculation.setValue(model.data.PlanPostPromoEffect);
                }
            }
        }

        if (durationStartDate.getValue() && durationEndDate.getValue()) {
            dispatchStartDate.reset();
            dispatchEndDate.reset();

            if (clientTreeRecord) {
                var isBeforeStart = clientTreeRecord.IsBeforeStart;
                var daysStart = clientTreeRecord.DaysStart;
                var isDaysStart = clientTreeRecord.IsDaysStart;
                var daysForDispatchStart = clientTreeRecord.DaysStart;
                var isBeforeEnd = clientTreeRecord.IsBeforeEnd;
                var daysEnd = clientTreeRecord.DaysEnd;
                var isDaysEnd = clientTreeRecord.IsDaysEnd;
                var daysForDispatchEnd = clientTreeRecord.DaysEnd;

                if (isBeforeStart !== null && daysStart !== null && isDaysStart !== null) {
                    var resultDateForDispatchStart = null;

                    if (!isDaysStart) {
                        daysForDispatchStart *= 7;
                    }

                    if (isBeforeStart) {
                        resultDateForDispatchStart = Ext.Date.add(durationStartDate.getValue(), Ext.Date.DAY, -daysForDispatchStart);
                    } else {
                        resultDateForDispatchStart = Ext.Date.add(durationStartDate.getValue(), Ext.Date.DAY, daysForDispatchStart);
                    }

                    if (resultDateForDispatchStart) {
                        dispatchStartDate.setValue(resultDateForDispatchStart);
                        dispatchEndDate.setMinValue(dispatchStartDate.getValue())
                    }
                }

                if (isBeforeEnd !== null && daysEnd !== null && isDaysEnd !== null) {
                    var resultDateForDispatchEnd = null;

                    if (!isDaysEnd) {
                        daysForDispatchEnd *= 7;
                    }

                    if (isBeforeEnd) {
                        resultDateForDispatchEnd = Ext.Date.add(durationEndDate.getValue(), Ext.Date.DAY, -daysForDispatchEnd);
                    } else {
                        resultDateForDispatchEnd = Ext.Date.add(durationEndDate.getValue(), Ext.Date.DAY, daysForDispatchEnd);
                    }

                    if (resultDateForDispatchEnd) {
                        dispatchEndDate.setValue(resultDateForDispatchEnd);
                    }
                }

                if (!dispatchStartDate.getValue() || !dispatchEndDate.getValue()) {
                    periodButton.addCls('notcompleted');
                    periodButton.setGlyph(0xf130);
                }

                var durationPeriod = 'c ' + Ext.Date.format(durationStartDate.getValue(), "d.m.Y") + ' по ' + Ext.Date.format(durationEndDate.getValue(), "d.m.Y"),
                    dispatchPeriod = dispatchStartDate.getValue() && dispatchEndDate.getValue() ? 'c ' + Ext.Date.format(dispatchStartDate.getValue(), "d.m.Y") + ' по ' + Ext.Date.format(dispatchEndDate.getValue(), "d.m.Y") : '',
                    text = '<b>' + l10n.ns('tpm', 'promoStap').value('basicStep4') + '</b><br><p>Promo: ' + durationPeriod + '<br>Dispatch: ' + dispatchPeriod + '</p>';

                periodButton.setText(text);
            }
        }

        me.setInfoPromoBasicStep1(promoClientForm);
    },

    afterInitClient: function (promoClientRecord, promoRecord, window, isCopy) {
        // Legacy code
        if (promoClientRecord) {
            var schedulerController = this.getController('tpm.schedule.SchedulerViewController');
            var dispatchStart = null;
            var dispatchEnd = null;
            var daysForDispatchStart = null;
            var daysForDispatchEnd = null;

            var period = window.down('container[name=promo_step4]');
            var dispatchStartDate = period.down('datefield[name=DispatchStartDate]');
            var dispatchEndDate = period.down('datefield[name=DispatchEndDate]');

            // period
            var durationStartDate = period.down('datefield[name=DurationStartDate]');
            var durationEndDate = period.down('datefield[name=DurationEndDate]');

            /// Сомнительный участок
            daysForDispatchStart = schedulerController.getDaysForDispatchDateFromClientSettings(
                promoClientRecord.IsBeforeStart, promoClientRecord.DaysStart, promoClientRecord.IsDaysStart);

            daysForDispatchEnd = schedulerController.getDaysForDispatchDateFromClientSettings(
                promoClientRecord.IsBeforeEnd, promoClientRecord.DaysEnd, promoClientRecord.IsDaysEnd);

            if (daysForDispatchStart !== null) {
                dispatchStart = Ext.Date.add(durationStartDate.getValue(), Ext.Date.DAY,
                    schedulerController.getDaysForDispatchDateFromClientSettings(
                        promoClientRecord.IsBeforeStart, promoClientRecord.DaysStart, promoClientRecord.IsDaysStart));
            }

            if (daysForDispatchEnd !== null) {
                dispatchEnd = Ext.Date.add(durationEndDate.getValue(), Ext.Date.DAY,
                    schedulerController.getDaysForDispatchDateFromClientSettings(
                        promoClientRecord.IsBeforeEnd, promoClientRecord.DaysEnd, promoClientRecord.IsDaysEnd));
            }

            if (isCopy) {
                // Берутся настройки dispatch у клиента, на которого копируется промо в календаре
                dispatchStartDate.setValue(dispatchStart);
                dispatchEndDate.setValue(dispatchEnd);
            } else {
                // Берутся настройки dispatch у текущей записи, в том числе при открытии на редактирование и просмотр.
                // Наличие или отсутсвие dispatch настроек у клиента не влияет на это действие.
                dispatchStartDate.setValue(promoRecord.data.DispatchesStart);
                dispatchEndDate.setValue(promoRecord.data.DispatchesEnd);
            }

            var promoPeriod = window.down('promoperiod');
            var durationStartDate = promoPeriod.down('datefield[name=DurationStartDate]');
            var durationEndDate = promoPeriod.down('datefield[name=DurationEndDate]');

            var currentDate = new Date();
            currentDate.setHours(0, 0, 0, 0);
        }
    },

    // --------------------- Выбор продуктов -----------------//

    // событие нажатия кновки выбора продуктов
    onСhoosePromoProductsBtnClick: function (button) {
        var me = this;
        var promoProductsForm = button.up('promobasicproducts');

        promoProductsForm.chooseProducts(function (nodesProductTree) {
            promoProductsForm.fillForm(nodesProductTree);
            me.setInfoPromoBasicStep2(promoProductsForm);
        });
    },

    // событие обработки галочек в дереве продуктов
    onProductTreeCheckChange: function (item, checked, eOpts) {
        var store = item.store;
        var treegrid = store.ownerTree;
        var productTree = treegrid.up('producttree');
        var nodes = treegrid.getRootNode().childNodes;
        var c = this.getController('tpm.product.ProductTree');

        // если узел имеет тип subrange, то ищем отмеченные узлы на том же уровне        
        var multiCheck = false;

        if (item.get('Type').indexOf('Subrange') >= 0) {
            for (var i = 0; i < item.parentNode.childNodes.length && !multiCheck; i++) {
                var node = item.parentNode.childNodes[i];
                multiCheck = node !== item && node.get('checked') && node.get('Type').indexOf('Subrange') >= 0;
            }
        }
        // если нельзя отметить несколько, сбрасываем галочки
        if (!multiCheck) {
            c.setCheckTree(nodes, false);
            productTree.choosenClientObjectId = [];
        }

        if (checked) {
            item.set('checked', true);
            productTree.choosenClientObjectId.push(item.get('ObjectId'));

            var parent = store.getNodeById(item.get('parentId'));
            if (parent) {
                while (parent.data.root !== true) {
                    parent.set('checked', true);
                    parent = store.getNodeById(parent.get('parentId'));
                }
            }
        }
        else {
            item.set('checked', false);

            var indexObjectId = productTree.choosenClientObjectId.indexOf(item.data.ObjectId);
            if (indexObjectId >= 0) {
                productTree.choosenClientObjectId.splice(indexObjectId, 1);
            }
        }

        //Если не начально загруженная запись - заблокировать кнопку productList
        //if (productButton.up('promoeditorcustom').productsSetted) {
        //    var productListButton = producttree.down('#productList');
        //    productListButton.setDisabled(true);
        //}
    },

    // событие нажатия кнопки Choose в окне выбора продуктов
    onChooseProductTreeClick: function (button) {
        var promoProductChooseWnd = button.up('promoproductchoosewindow');
        var productTree = promoProductChooseWnd.down('producttree');

        // если есть call-back функция, то вызываем её, возвращая выбранного клиента
        if (promoProductChooseWnd.callBackChooseFnc) {
            var productTreeGrid = productTree.down('producttreegrid');
            var productTreeStore = productTreeGrid.getStore();
            // выбранные узлы
            var productTreeNodes = [];

            // ищем записи по ObjectId в Store
            productTree.choosenClientObjectId.forEach(function (item) {
                var record = productTreeStore.getById(item);
                productTreeNodes.push(record)
            });
            
            promoProductChooseWnd.callBackChooseFnc(productTreeNodes);
        }

        promoProductChooseWnd.close();
    },

    // устанавливаем информацию на кнопке Step2 Basic Promo
    // а также меняем продукты в записи окна
    setInfoPromoBasicStep2: function (promoProductForm) {
        var promoWindow = promoProductForm.up('promoeditorcustom');
        var productButton = promoWindow.down('button#btn_promo_step2');

        // если клиент выбран, то clientTreeRecord != null
        if (promoProductForm.choosenProductObjectIds.length > 0) {
            var text = '<b>' + l10n.ns('tpm', 'promoStap').value('basicStep2') + '</b><br><p>';
            var displayHierarchy = promoProductForm.fullPath.replace(/>/g, '<span style="font-size: 13px; font-family: MaterialDesignIcons; color: #A7AFB7"> </span>');


            productButton.setText(text + displayHierarchy + '</p>');
            productButton.removeCls('notcompleted');
            productButton.setGlyph(0xf133);
            productButton.isComplete = true;
            promoWindow.productTreeNodes = promoProductForm.choosenProductObjectIds;
            promoWindow.productHierarchy = promoProductForm.fullPath;
        }
        else {
            productButton.setText('<b>' + l10n.ns('tpm', 'promoStap').value('basicStep2') + '</b>');
            productButton.addCls('notcompleted');
            productButton.setGlyph(0xf130);
            productButton.isComplete = false;
            promoWindow.productTreeNodes = [];
            promoWindow.productHierarchy = null;
        }

        // проверяем готовность Promo Basic        
        checkMainTab(productButton.up(), promoWindow.down('button#btn_promo'));
    },

    // простмотр продуктов под выбранные Subranges
    onFilteredListBtnClick: function (button) {
        var promoProductsForm = button.up('promobasicproducts');
        var filter = promoProductsForm.getFilterForSubranges(); // фильтр в JSON, но не пригодный для store

        if (filter) {
            var productlist = Ext.widget('productlist', { title: l10n.ns('tpm', 'button').value('FilteredProductList') });                
            var grid = productlist.down('directorygrid');
            var store = grid.getStore();
            var extendedFilter = store.extendedFilter;

            extendedFilter.filter = filter;
            extendedFilter.reloadStore();

            productlist.show();
        } else {
            App.Notify.pushInfo('For one or more choosen product nodes, the filter is empty');
        }
    },

    // просмотр зафиксированных продуктов
    onProductListBtnClick: function (button) {
        var productlist = Ext.widget('actualproductlist');
        var promogrid = button.up('promoeditorcustom');
        var grid = productlist.down('directorygrid');
        var store = grid.getStore();

        store.setFixedFilter('PromoId', {
            property: 'PromoId',
            operation: 'Equals',
            value: promogrid.promoId
        });

        productlist.show();
    },

    checkLogForErrors: function (recordId) {
        var parameters = {
            promoId: recordId
        };
        App.Util.makeRequestWithCallback('Promoes', 'CheckIfLogHasErrors', parameters, function (data) {
            var result = Ext.JSON.decode(data.httpResponse.data.value);
            var but = Ext.ComponentQuery.query('promoeditorcustom #btn_showlog')[0];
            if (result.LogHasErrors) {
                but.addCls('errorinside');
            } else {
                but.removeCls('errorinside');
            }
        })
    }
});