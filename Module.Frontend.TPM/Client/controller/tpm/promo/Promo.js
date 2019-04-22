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
                'promoeditorcustom #btn_close': {
                    click: this.onToClosePromoButtonClick
                },
                'promoeditorcustom #btn_reject': {
                    click: this.onRejectButtonClick
                },
                'promoeditorcustom #btn_rejectPlan': {
                    click: this.onRejectButtonClick
                },
                'promoeditorcustom #btn_history': {
                    click: this.onPromoHistoryButtonClick
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

                // clienttree
                'promoeditorcustom clienttree basetreegrid': {
                    checkchange: this.onClientTreeCheckChange
                },

                // producttree
                'promoeditorcustom producttree basetreegrid': {
                    checkchange: this.onProductTreeCheckChange
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
                'promoeditorcustom #basicPromoPanel': {
                    afterrender: this.onBasicPanelAfterrender
                }
            }
        });
    },

    onGridPromoAfterrender: function (grid) {
        var store = grid.getStore();

        store.on('load', function (store) {
            var selectionModel = grid.getSelectionModel();
            if (selectionModel.hasSelection()) {
                var view = grid.getView(),
                    jspData = $(view.getEl().dom).data('jsp'),
                    count = store.totalCount;

                if (view && view.prevOffsetTop) {
                    var k = 25 * count / 519,
                        scrollValue = view.prevOffsetTop * k;
                    jspData.scrollToY(scrollValue, false);
                    view.prevOffsetTop = 0;
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

                var h1 = formStep1.up().height;
                var h1_2 = h1 + formStep2.up().height;
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

                    formStep1.up().header.addClass('promo-header-item-active');
                    formStep2.up().header.removeCls('promo-header-item-active');
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

                    formStep1.up().header.removeCls('promo-header-item-active');
                    formStep2.up().header.addClass('promo-header-item-active');
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

                    formStep1.up().header.removeCls('promo-header-item-active');
                    formStep2.up().header.removeCls('promo-header-item-active');
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

                    formStep1.up().header.removeCls('promo-header-item-active');
                    formStep2.up().header.removeCls('promo-header-item-active');
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

                    formStep1.up().header.removeCls('promo-header-item-active');
                    formStep2.up().header.removeCls('promo-header-item-active');
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

                    formStep1.up().header.removeCls('promo-header-item-active');
                    formStep2.up().header.removeCls('promo-header-item-active');
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
        var me = this;
        var promoeditorcustom = button.up('promoeditorcustom');
        var mask = promoeditorcustom.setLoading(true);
        // для того чтобы маска отрисовалась первой
        setTimeout(function () {
            me.fillSummaryPanel(button, mask);
        }, 0);
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
        var factInc = record.get('FactPromoIncrementalLSV'),
            factBL = record.get('FactPromoBaselineLSV'),
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
            { type: 'Fact', shopper: factShopper, marketing: factMarketing, cost: factCost, brand: factBranding, btl: factBTL }
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
            brandTechLabel = promoInformationPanel.down('label[name=brandTechLabel]'),
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
        brandTechLabel.setText(record.get('BrandTechName'));

        var dispatchDatesText = Ext.String.format('{0} - {1}', Ext.Date.format(record.get('DispatchesStart'), 'd.m.Y'), Ext.Date.format(record.get('DispatchesEnd'), 'd.m.Y'));
        dispatchesDatesLabel.setText(dispatchDatesText);
        summaryStatusField.update({ StatusColor: record.get('PromoStatusColor'), StatusName: record.get('PromoStatusName') });
        promoEventNameButton.setText(record.get('PromoEventName'));
    },

    // Обновление текстовых данных план/факт 
    updateSummaryPlanFactLabels: function (summary, record) {
        var me = this;
        var planROI = record.get('PlanPromoROIPercent') || 0,
            factROI = record.get('ActualPromoROIPercent') || 0,
            template = '<span class="promo-value-{0}">{1}%</span>{2}';
        planROI = Ext.util.Format.round(planROI, 2);
        factROI = Ext.util.Format.round(planROI, 2);
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
        var el = $(container.down('producttree[itemId=promo_step2]').up().getTargetEl().dom);
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

    onCreateButtonClick: function (button, e, schedulerData) {
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
                        var c = me.getController('tpm.client.ClientTree');
                        var client = promoeditorcustom.down('container[name=promo_step1]');
                        var clienttreegrid = client.down('clienttreegrid');
                        var clienttreestore = clienttreegrid.store;
                        var durationDateStart = period.down('datefield[name=DurationStartDate]');
                        var durationDateEnd = period.down('datefield[name=DurationEndDate]');
                        var startDate = schedulerData.schedulerContext.start;
                        var endDate = schedulerData.schedulerContext.end;
                        var clientTreeId;

                        if (schedulerData.isCopy) { }
                        else {
                            clientTreeId = schedulerData.schedulerContext.resourceRecord.getId();
                        }
                        var clienttreegrid = client.down('clienttreegrid');
                        var clienttreestore = clienttreegrid.store;

                        clienttreestore.getProxy().extraParams.clientObjectId = clientTreeId;
                        clienttreestore.load();
                        durationDateStart.setValue(startDate);
                        durationDateEnd.setValue(endDate);
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
                    }

                    promoeditorcustom.show();
                    parentWidget.setLoading(false);
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    parentWidget.setLoading(false);
                    App.Notify.pushError(l10n.ns('tpm', 'text').value('failedStatusLoad'));
                }
            });
        }
    },

    onUpdateButtonClick: function (button) {
        var grid = this.getGridByButton(button);
        var promoeditorcustom = Ext.widget('promoeditorcustom');
        this.detailButton = null;

        // Если запись обозначена
        if (button.assignedRecord) {
            promoeditorcustom.isCreating = false;
            promoeditorcustom.assignedRecord = button.assignedRecord;

            this.bindAllLoadEvents(promoeditorcustom, button.assignedRecord, false);
            this.fillPromoForm(promoeditorcustom, button.assignedRecord, false, false, grid);
            // Редактирование выбранной записи
        } else {
            this.getScrollPosition(button);

            var selectionModel = grid.getSelectionModel();
            if (selectionModel.hasSelection()) {
                var record = selectionModel.getSelection()[0];
                promoeditorcustom.isCreating = false;

                this.bindAllLoadEvents(promoeditorcustom, record, false);
                this.fillPromoForm(promoeditorcustom, record, false, false, grid);
            } else {
                grid.up('promo').setLoading(false);
                App.Notify.pushError(l10n.ns('tpm', 'text').value('notSelected'));
            }
        }

        //Для блокирования кнопки продуктов
        var productTreeGrid = promoeditorcustom.down('producttreegrid');
        var productTreeStore = productTreeGrid.getStore();
        productTreeStore.on('load', function (store, node, records, success) {
            promoeditorcustom.productsSetted = true;
        });

        //Установка readOnly полям, для которых текущая роль не входит в crudAccess
        this.setFieldsReadOnlyForSomeRole(promoeditorcustom);
    },

    onDetailButtonClick: function (button) {
        var promoeditorcustom = Ext.widget('promoeditorcustom');
        this.getController('tpm.promo.Promo').detailButton = button;

        // Если запись обозначена
        if (button.assignedRecord) {
            promoeditorcustom.isCreating = false;
            promoeditorcustom.assignedRecord = button.assignedRecord;

            this.bindAllLoadEvents(promoeditorcustom, button.assignedRecord, false);
            this.fillPromoForm(promoeditorcustom, button.assignedRecord, true);
            // Просмотр выбранной записи
        } else {
            this.getScrollPosition(button);

            var grid = this.getGridByButton(button);
            var selectionModel = grid.getSelectionModel();
            if (selectionModel.hasSelection()) {
                var record = selectionModel.getSelection()[0];
                promoeditorcustom.isCreating = false;

                this.bindAllLoadEvents(promoeditorcustom, record, false);
                this.fillPromoForm(promoeditorcustom, record, true, false, grid);
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

    onChangePromoButtonClick: function (button) {
        var window = button.up('promoeditorcustom');
        window.setLoading(true, undefined, true);
        var me = this;
        setTimeout(function () {
            var promoeditorcustom = button.up('window');
            promoeditorcustom.readOnly = false;

            var customtoptoolbar = promoeditorcustom.down('customtoptoolbar');

            var client = promoeditorcustom.down('container[name=promo_step1]');
            var product = promoeditorcustom.down('container[name=promo_step2]');
            var mechanic = promoeditorcustom.down('container[name=promo_step3]');
            var period = promoeditorcustom.down('container[name=promo_step4]');
            var event = promoeditorcustom.down('container[name=promo_step5]');
            var settings = promoeditorcustom.down('container[name=promo_step6]');

            //var promocalculation = promoeditorcustom.down('promocalculation');

            //var costAndBudget = promocalculation.down('container[name=costAndBudget]');
            //var activity = promocalculation.down('container[name=activity]');
            //var financialIndicator = promocalculation.down('container[name=financialIndicator]');
            var currentRole = App.UserInfo.getCurrentRole()['SystemName'];

            // --------------- basic promo ---------------

            // Client tree
            var clientCrudAccess = ['Administrator', 'FunctionalExpert', 'CustomerMarketing', 'KeyAccountManager'];
            if (clientCrudAccess.indexOf(currentRole) > -1) {
                var window = button.up('promoeditorcustom');
                var record = me.getRecord(window);

                var clientTreeGrid = client.down('clienttreegrid');
                var clientTreeStore = clientTreeGrid.getStore();
                var clientTreeId = record.get('ClientTreeId');

                clientTreeStore.getRootNode().removeAll();
                clientTreeStore.getRootNode().setId('root');

                if (clientTreeId) {
                    clientTreeStore.getProxy().extraParams.clientObjectId = record.get('ClientTreeId');
                    clientTreeStore.load();
                }

                clientTreeGrid.setDisabled(clientTreeGrid.blockChange);
            }

            // Product tree
            var productCrudAccess = ['Administrator', 'FunctionalExpert', 'CustomerMarketing', 'KeyAccountManager'];
            if (productCrudAccess.indexOf(currentRole) > -1) {
                var productTreeGrid = product.down('producttreegrid');
                var productTreeStore = productTreeGrid.getStore();

                productTreeStore.on('load', function (store, node, records, success) {
                    window.productsSetted = true;
                });

                productTreeStore.getRootNode().removeAll();
                productTreeStore.getRootNode().setId('root');

                if (clientTreeId) {
                    productTreeStore.getProxy().extraParams.promoId = record.get('Id');
                    productTreeStore.load();
                    productTreeStore.getProxy().extraParams.promoId = null;
                }

                productTreeGrid.setDisabled(productTreeGrid.blockChange);
            }

            // --------------- buttons ---------------

            //uplift
            promoeditorcustom.down('button[itemId=savePromo]').show();
            promoeditorcustom.down('button[itemId=saveAndClosePromo]').show();
            promoeditorcustom.down('button[itemId=cancelPromo]').show();
            promoeditorcustom.down('button[itemId=closePromo]').hide();
            promoeditorcustom.down('button[itemId=changePromo]').hide();

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

            //Установка readOnly полям, для которых текущая роль не входит в crudAccess
            me.setFieldsReadOnlyForSomeRole(promoeditorcustom);

            me.validatePromoModel(promoeditorcustom);

            // Разблокировка кнопок Add Promo Support
            var promoBudgets = button.up('window').down('promobudgets');
            var addSubItemButtons = promoBudgets.query('#addSubItem');

            addSubItemButtons.forEach(function (button) {
                button.setDisabled(false);
            })

            // Сброк полей Instore Mechanic
            var promoController = App.app.getController('tpm.promo.Promo'),
                promoMechanic = Ext.ComponentQuery.query('promomechanic')[0],
                mechanicFields = promoController.getMechanicFields(promoMechanic);

            promoController.resetFields([
                mechanicFields.instoreMechanicFields.instoreMechanicId,
                mechanicFields.instoreMechanicFields.instoreMechanicTypeId,
                mechanicFields.instoreMechanicFields.instoreMechanicDiscount,
            ]);

            promoController.disableFields([
                mechanicFields.instoreMechanicFields.instoreMechanicTypeId,
                mechanicFields.instoreMechanicFields.instoreMechanicDiscount,
            ]);

            //Установка полей механик
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

        }, 5);
        window.setLoading(false, undefined, false);
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
                    var selectionModel = grid.getSelectionModel();
                    if (selectionModel.hasSelection()) {
                        var record = selectionModel.getSelection()[0];
                        window.readOnly = true;
                        me.reFillPromoForm(window, record, grid);
                    }
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
    },


    onPublishButtonClick: function (button) {
        var window = button.up('promoeditorcustom');

        var isStep1Complete = !window.down('#btn_promo_step1').hasCls('notcompleted');
        var isStep2Complete = !window.down('#btn_promo_step2').hasCls('notcompleted');
        var isStep3Complete = !window.down('#btn_promo_step3').hasCls('notcompleted');
        var isStep4Complete = !window.down('#btn_promo_step4').hasCls('notcompleted');
        var isStep5Complete = !window.down('#btn_promo_step5').hasCls('notcompleted');
        var isStep6Complete = !window.down('#btn_promo_step6').hasCls('notcompleted');

        if (isStep1Complete && isStep2Complete && isStep3Complete && isStep4Complete && isStep5Complete && isStep6Complete) {
            if (this.validatePromoModel(window)) {
                var record = this.getRecord(window);

                window.statusId = button.statusId;
                window.promoName = this.getPromoName(window);

                var model = this.buildPromoModel(window, record);
                this.saveModel(model, window, false, true);
            } else {
                return;
            }
        } else {
            App.Notify.pushInfo(l10n.ns('tpm', 'text').value('completeSteps'));
        }
    },

    onUndoPublishButtonClick: function (button) {
        var window = button.up('promoeditorcustom');

        var isStep1Complete = !window.down('#btn_promo_step1').hasCls('notcompleted');
        var isStep2Complete = !window.down('#btn_promo_step2').hasCls('notcompleted');
        var isStep3Complete = !window.down('#btn_promo_step3').hasCls('notcompleted');
        var isStep4Complete = !window.down('#btn_promo_step4').hasCls('notcompleted');
        var isStep5Complete = !window.down('#btn_promo_step5').hasCls('notcompleted');
        var isStep6Complete = !window.down('#btn_promo_step6').hasCls('notcompleted');

        if (isStep1Complete && isStep2Complete && isStep3Complete && isStep4Complete && isStep5Complete && isStep6Complete) {
            if (this.validatePromoModel(window)) {
                var record = this.getRecord(window);

                window.statusId = button.statusId;
                window.promoName = 'Unpublish Promo';

                var model = this.buildPromoModel(window, record);
                this.saveModel(model, window, false, true);

                // если во время возврата была открыта вкладка Calculations/Activity нужно переключиться с них
                var btn_promo = button.up('window').down('container[name=promo]');
                //var btn_support = button.up('window').down('container[name=support]');

                if (!btn_promo.hasCls('selected')) {// && !btn_support.hasCls('selected')) {
                    this.onPromoButtonClick(btn_promo);
                }
            } else {
                return;
            }
        } else {
            App.Notify.pushInfo(l10n.ns('tpm', 'text').value('completeSteps'));
        }
    },

    onSendForApprovalButtonClick: function (button) {
        var window = button.up('promoeditorcustom');

        var isStep1Complete = !window.down('#btn_promo_step1').hasCls('notcompleted');
        var isStep2Complete = !window.down('#btn_promo_step2').hasCls('notcompleted');
        var isStep3Complete = !window.down('#btn_promo_step3').hasCls('notcompleted');
        var isStep4Complete = !window.down('#btn_promo_step4').hasCls('notcompleted');
        var isStep5Complete = !window.down('#btn_promo_step5').hasCls('notcompleted');
        var isStep6Complete = !window.down('#btn_promo_step6').hasCls('notcompleted');

        var isStep7Complete = !window.down('#btn_promoBudgets_step1').hasCls('notcompleted');
        var isStep8Complete = !window.down('#btn_promoBudgets_step2').hasCls('notcompleted');
        var isStep9Complete = !window.down('#btn_promoBudgets_step3').hasCls('notcompleted');

        if ((isStep1Complete && isStep2Complete && isStep3Complete && isStep4Complete && isStep5Complete && isStep6Complete) && (isStep7Complete && isStep8Complete && isStep9Complete)) {
            if (this.validatePromoModel(window)) {
                var record = this.getRecord(window);

                window.statusId = button.statusId;
                window.promoName = this.getPromoName(window);

                var model = this.buildPromoModel(window, record);
                this.saveModel(model, window, false, true);
            } else {
                return;
            }
        } else {
            App.Notify.pushInfo(l10n.ns('tpm', 'text').value('completeSteps'));
        }
    },

    onApproveButtonClick: function (button) {
        var window = button.up('promoeditorcustom');

        // TODO: необходимо точно выяснить ограничения
        var isStep1Complete = !window.down('#btn_promo_step1').hasCls('notcompleted');
        var isStep2Complete = !window.down('#btn_promo_step2').hasCls('notcompleted');
        var isStep3Complete = !window.down('#btn_promo_step3').hasCls('notcompleted');
        var isStep4Complete = !window.down('#btn_promo_step4').hasCls('notcompleted');
        var isStep5Complete = !window.down('#btn_promo_step5').hasCls('notcompleted');
        var isStep6Complete = !window.down('#btn_promo_step6').hasCls('notcompleted');

        var isStep7Complete = !window.down('#btn_promoBudgets_step1').hasCls('notcompleted');
        var isStep8Complete = !window.down('#btn_promoBudgets_step2').hasCls('notcompleted');
        var isStep9Complete = !window.down('#btn_promoBudgets_step3').hasCls('notcompleted');

        if ((isStep1Complete && isStep2Complete && isStep3Complete && isStep4Complete && isStep5Complete && isStep6Complete) && (isStep7Complete && isStep8Complete && isStep9Complete)) {
            if (this.validatePromoModel(window)) {
                var me = this;
                // окно подтверждения
                Ext.Msg.show({
                    title: l10n.ns('tpm', 'text').value('Confirmation'),
                    msg: l10n.ns('tpm', 'Promo').value('Confirm Approval'),
                    fn: function (btn) {
                        if (btn === 'yes') {
                            // Логика для согласования
                            var record = me.getRecord(window);

                            window.statusId = button.statusId;
                            window.promoName = me.getPromoName(window);

                            var model = me.buildPromoModel(window, record);

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
                return;
            }
        } else {
            App.Notify.pushInfo(l10n.ns('tpm', 'text').value('completeSteps'));
        }
    },

    onPlanButtonClick: function (button) {
        var window = button.up('promoeditorcustom');

        // TODO: необходимо точно выяснить ограничения
        var isStep1Complete = !window.down('#btn_promo_step1').hasCls('notcompleted');
        var isStep2Complete = !window.down('#btn_promo_step2').hasCls('notcompleted');
        var isStep3Complete = !window.down('#btn_promo_step3').hasCls('notcompleted');
        var isStep4Complete = !window.down('#btn_promo_step4').hasCls('notcompleted');
        var isStep5Complete = !window.down('#btn_promo_step5').hasCls('notcompleted');
        var isStep6Complete = !window.down('#btn_promo_step6').hasCls('notcompleted');

        var isStep7Complete = !window.down('#btn_promoBudgets_step1').hasCls('notcompleted');
        var isStep8Complete = !window.down('#btn_promoBudgets_step2').hasCls('notcompleted');
        var isStep9Complete = !window.down('#btn_promoBudgets_step3').hasCls('notcompleted');

        if ((isStep1Complete && isStep2Complete && isStep3Complete && isStep4Complete && isStep5Complete && isStep6Complete) && (isStep7Complete && isStep8Complete && isStep9Complete)) {
            if (this.validatePromoModel(window)) {
                var record = this.getRecord(window);
                var me = this;

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
                return;
            }
        } else {
            App.Notify.pushInfo(l10n.ns('tpm', 'text').value('completeSteps'));
        }
    },


    onToClosePromoButtonClick: function (button) {
        var window = button.up('promoeditorcustom');

        // TODO: необходимо точно выяснить ограничения
        var isStep1Complete = !window.down('#btn_promo_step1').hasCls('notcompleted');
        var isStep2Complete = !window.down('#btn_promo_step2').hasCls('notcompleted');
        var isStep3Complete = !window.down('#btn_promo_step3').hasCls('notcompleted');
        var isStep4Complete = !window.down('#btn_promo_step4').hasCls('notcompleted');
        var isStep5Complete = !window.down('#btn_promo_step5').hasCls('notcompleted');
        var isStep6Complete = !window.down('#btn_promo_step6').hasCls('notcompleted');

        var isStep7Complete = !window.down('#btn_promoBudgets_step1').hasCls('notcompleted');
        var isStep8Complete = !window.down('#btn_promoBudgets_step2').hasCls('notcompleted');
        var isStep9Complete = !window.down('#btn_promoBudgets_step3').hasCls('notcompleted');

        if ((isStep1Complete && isStep2Complete && isStep3Complete && isStep4Complete && isStep5Complete && isStep6Complete) && (isStep7Complete && isStep8Complete && isStep9Complete)) {
            if (this.validatePromoModel(window)) {
                var record = this.getRecord(window);
                var me = this;

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
                return;
            }
        } else {
            App.Notify.pushInfo(l10n.ns('tpm', 'text').value('completeSteps'));
        }
    },

    // =============== Other ===============

    validatePromoModel: function (window) {
        var promomechanic = window.down('promomechanic');
        var promoperiod = window.down('promoperiod');
        var promosettings = window.down('promosettings');

        var v1 = promomechanic.down('numberfield[name=MarsMechanicDiscount]').validate();
        var v2 = promomechanic.down('numberfield[name=PlanInstoreMechanicDiscount]').validate();
        var v3 = promomechanic.down('textarea[name=PromoComment]').validate();

        var v4 = promoperiod.down('datefield[name=DurationStartDate]').validate();
        var v5 = promoperiod.down('datefield[name=DurationEndDate]').validate();
        var v6 = promoperiod.down('datefield[name=DispatchStartDate]').validate();
        var v7 = promoperiod.down('datefield[name=DispatchEndDate]').validate();

        var v8 = promosettings.down('sliderfield[name=priority]').validate();

        if (v1 && v2 && v3 && v4 && v5 && v6 && v7 && v8) {
            return true;
        } else {
            return false;
        }
    },

    buildPromoModel: function (window, record) {
        // basic promo
        var clienttree = window.down('clienttree');
        var producttree = window.down('producttree');
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

        // clienttree
        record.data.ClientTreeId = window.clientTreeId;
        record.data.ClientHierarchy = window.clientHierarchy;

        // producttree
        record.data.BrandId = window.brandId;
        record.data.TechnologyId = window.technologyId;
        record.data.ProductHierarchy = window.productHierarchy;

        record.data.ProductTreeObjectIds = '';
        window.productTreeNodes.forEach(function (node) {
            record.data.ProductTreeObjectIds += node.get('ObjectId') + ';';
        });
        record.data.ProductTreeObjectIds = record.data.ProductTreeObjectIds.slice(0, -1);

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
        //record.data.PlanPostPromoEffect = activity.down('trigger[name=PlanPostPromoEffectTotal]').getValue();

        //record.data.ActualPromoUpliftPercent = activity.down('numberfield[name=ActualPromoUpliftPercent]').getValue();
        //record.data.ActualPromoIncrementalLSV = activity.down('numberfield[name=ActualPromoIncrementalLSV]').getValue();
        //record.data.ActualPromoLSV = activity.down('numberfield[name=ActualPromoLSV]').getValue();
        //record.data.FactPostPromoEffect = activity.down('trigger[name=FactPostPromoEffectTotal]').getValue();

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
        record.data.ActualInStoreMechanicDiscount = promoActivityStep1.down('numberfield[name=ActualInstoreMechanicDiscount]').getValue();

        record.data.ActualInStoreShelfPrice = promoActivityStep1.down('numberfield[name=ActualInStoreShelfPrice]').getValue();
        record.data.InvoiceNumber = promoActivityStep1.down('textfield[name=InvoiceNumber]').getValue();

        record.data.PlanPromoUpliftPercent = promoActivityStep2.down('numberfield[name=PlanPromoUpliftPercent]').getValue();
        //record.data.PlanPromoBaselineLSV = promoActivityStep2.down('numberfield[name=PlanPromoBaselineLSV]').getValue();
        //record.data.PlanPromoIncrementalLSV = promoActivityStep2.down('numberfield[name=PlanPromoIncrementalLSV]').getValue();
        //record.data.PlanPromoLSV = promoActivityStep2.down('numberfield[name=PlanPromoLSV]').getValue();
        //record.data.PlanPostPromoEffectTotal = promoActivityStep2.down('numberfield[name=PlanPostPromoEffectTotal]').getValue();

        //record.data.ActualPromoUpliftPercent = promoActivityStep2.down('numberfield[name=ActualPromoUpliftPercent]').getValue();
        //record.data.ActualPromoBaselineLSV = promoActivityStep2.down('numberfield[name=ActualPromoBaselineLSV]').getValue();
        //record.data.ActualPromoIncrementalLSV = promoActivityStep2.down('numberfield[name=ActualPromoIncrementalLSV]').getValue();
        //record.data.ActualPromoLSV = promoActivityStep2.down('numberfield[name=ActualPromoLSV]').getValue();
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
        var calculating = record.get('Calculating');

        readOnly = readOnly || calculating;
        promoeditorcustom.readOnly = readOnly;

        // для блокировки/разблокировки грида/календаря
        // Если кнопка не null, то из грида, иначе из календаря
        var parentWidget = promoGrid ? promoGrid.up('promo') : Ext.ComponentQuery.query('schedulecontainer')[0];
        parentWidget.setLoading(true);

        $.ajax({
            dataType: 'json',
            url: '/odata/PromoStatuss',
            success: function (promoStatusData) {
                var customtoptoolbar = promoeditorcustom.down('customtoptoolbar');
                var promoController = App.app.getController('tpm.promo.Promo');

                // --------------- basic promo ---------------
                var client = promoeditorcustom.down('container[name=promo_step1]');
                var product = promoeditorcustom.down('container[name=promo_step2]');
                var mechanic = promoeditorcustom.down('container[name=promo_step3]');
                var period = promoeditorcustom.down('container[name=promo_step4]');
                var event = promoeditorcustom.down('container[name=promo_step5]');
                var settings = promoeditorcustom.down('container[name=promo_step6]');

                // client
                var clienttreegrid = client.down('clienttreegrid');

                // product
                var producttreegrid = product.down('producttreegrid');

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
                var actualInstoreMechanicDiscount = promoActivityStep1.down('numberfield[name=ActualInstoreMechanicDiscount]');

                var actualInStoreShelfPrice = promoActivityStep1.down('numberfield[name=ActualInStoreShelfPrice]');
                var invoiceNumber = promoActivityStep1.down('textfield[name=InvoiceNumber]');

                var planPromoUpliftPercent = promoActivityStep2.down('[name=PlanPromoUpliftPercent]');
                var promoUpliftLockedUpdateCheckbox = promoActivityStep2.down('checkbox[itemId=PromoUpliftLockedUpdateCheckbox]');
                var planPromoBaselineLSV = promoActivityStep2.down('[name=PlanPromoBaselineLSV]');
                var planPromoIncrementalLSV = promoActivityStep2.down('[name=PlanPromoIncrementalLSV]');
                var planPromoLSV = promoActivityStep2.down('[name=PlanPromoLSV]');
                var planPostPromoEffect = promoActivityStep2.down('[name=PlanPostPromoEffect]');

                var actualPromoUpliftPercent = promoActivityStep2.down('[name=ActualPromoUpliftPercent]');
                var actualPromoBaselineLSV = promoActivityStep2.down('[name=ActualPromoBaselineLSV]');
                var actualPromoIncrementalLSV = promoActivityStep2.down('[name=ActualPromoIncrementalLSV]');
                var actualPromoLSV = promoActivityStep2.down('[name=ActualPromoLSV]');
                var factPostPromoEffect = promoActivityStep2.down('[name=FactPostPromoEffect]');

                // Блокировка изменения значений
                if (readOnly) {
                    // --------------- basic promo ---------------
                    // client
                    clienttreegrid.setDisabled(true);

                    // product
                    producttreegrid.setDisabled(true);

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
                    promoeditorcustom.down('button[itemId=changePromo]').show();
                }

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
                        var btn_publish = promoeditorcustom.down('button[itemId=btn_sendForApproval]');
                        btn_publish.statusId = promoStatusData.value[i].Id;
                        btn_publish.statusName = promoStatusData.value[i].Name;
                        btn_publish.statusSystemName = promoStatusData.value[i].SystemName;
                    }

                    if (promoStatusData.value[i].SystemName == 'Approved') {
                        var btn_approve = promoeditorcustom.down('button[itemId=btn_approve]');
                        btn_approve.statusId = promoStatusData.value[i].Id;
                        btn_approve.statusName = promoStatusData.value[i].Name;
                        btn_approve.statusSystemName = promoStatusData.value[i].SystemName;
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

                    if (promoStatusData.value[i].SystemName == 'Draft') {
                        draftStatusData = promoStatusData.value[i];

                        var undoBtn = promoeditorcustom.down('button[itemId=btn_undoPublish]');
                        undoBtn.statusId = promoStatusData.value[i].Id;
                        undoBtn.statusName = promoStatusData.value[i].Name;
                        undoBtn.statusSystemName = promoStatusData.value[i].SystemName;
                    }
                }

                if (isCopy) {
                    promoeditorcustom.down('button[itemId=btn_promoBudgets]').setDisabled(true);
                    promoeditorcustom.down('button[itemId=btn_promoBudgets]').addCls('disabled');
                    promoeditorcustom.down('button[itemId=btn_promoActivity]').setDisabled(true);
                    promoeditorcustom.down('button[itemId=btn_promoActivity]').addCls('disabled');
                    promoeditorcustom.statusId = draftStatusData.Id;
                    promoeditorcustom.promoStatusName = draftStatusData.Name;
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
                var clientTreeId = isCopy ? record.schedulerContext.resourceRecord.getId() : record.data.ClientTreeId;
                var clienttreestore = clienttreegrid.store
                var currentRole = App.UserInfo.getCurrentRole()['SystemName'];

                if (clientTreeId != null) {
                    if (treesChangingBlockDate) {
                        clienttreestore.getProxy().extraParams.dateFilter = treesChangingBlockDate;
                        clienttreegrid.blockChange = true;
                    } else {
                        clienttreegrid.blockChange = false;
                        clienttreestore.getProxy().extraParams.dateFilter = null;
                    }
                    clienttreestore.getProxy().extraParams.clientObjectId = clientTreeId;

                    var clientCrudAccess = ['Administrator', 'FunctionalExpert', 'CustomerMarketing', 'KeyAccountManager'];

                    if (clientCrudAccess.indexOf(currentRole) > -1) {
                        clienttreestore.getProxy().extraParams.view = readOnly;
                    } else {
                        clienttreestore.getProxy().extraParams.view = true;
                    }

                    clienttreestore.load();
                    clienttreestore.getProxy().extraParams.view = false;
                } else {
                    clienttreestore.load();
                }

                // product
                var promoId = record.data.Id;
                var producttreestore = producttreegrid.store;

                if (treesChangingBlockDate) {
                    producttreestore.getProxy().extraParams.dateFilter = treesChangingBlockDate;
                    producttreegrid.blockChange = true;
                } else {
                    producttreegrid.blockChange = false;
                    producttreestore.getProxy().extraParams.dateFilter = null;
                }

                producttreestore.getProxy().extraParams.promoId = promoId;

                var productCrudAccess = ['Administrator', 'FunctionalExpert', 'CustomerMarketing', 'KeyAccountManager'];

                if (productCrudAccess.indexOf(currentRole) > -1) {
                    producttreestore.getProxy().extraParams.view = readOnly;
                } else {
                    producttreestore.getProxy().extraParams.view = true;
                }

                producttreestore.load();
                producttreestore.getProxy().extraParams.view = false;
                product.down('#productList').setVisible(true);

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
                shopperTi.setValue(record.data.PlanPromoTIShopper);
                marketingTi.setValue(record.data.PlanPromoTIMarketing);
                branding.setValue(record.data.PlanPromoBranding);
                totalCost.setValue(record.data.PlanPromoCost);
                btl.setValue(record.data.PlanPromoBTL);
                costProduction.setValue(record.data.PlanPromoCostProduction);

                actualPromoTIShopper.setValue(record.data.ActualPromoTIShopper);
                actualPromoTIMarketing.setValue(record.data.ActualPromoTIMarketing);
                actualPromoBranding.setValue(record.data.ActualPromoBranding);
                factTotalCost.setValue(record.data.ActualPromoCost);
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
                        Name: record.data.PlanInstoreMechanicTypeName
                    }));
                    actualInstoreMechanicDiscount.setValue(record.data.ActualInStoreDiscount);
                } else if (record.data.ActualInStoreMechanicDiscount) {
                    actualInstoreMechanicDiscount.setValue(record.data.ActualInStoreDiscount);
                }

                actualInStoreShelfPrice.setValue(record.data.ActualInStoreShelfPrice);
                invoiceNumber.setValue(record.data.InvoiceNumber);

                if (actualInstoreMechanicId.crudAccess.indexOf(currentRole) === -1) {
                    actualInstoreMechanicId.setReadOnly(true);
                }
                if (actualInstoreMechanicTypeId.crudAccess.indexOf(currentRole) === -1) {
                    actualInstoreMechanicTypeId.setReadOnly(true);
                }
                if (actualInstoreMechanicDiscount.crudAccess.indexOf(currentRole) === -1) {
                    actualInstoreMechanicDiscount.setReadOnly(true);
                }

                if (actualInStoreShelfPrice.crudAccess.indexOf(currentRole) === -1) {
                    actualInStoreShelfPrice.setReadOnly(true);
                }

                if (invoiceNumber.crudAccess.indexOf(currentRole) === -1) {
                    invoiceNumber.setReadOnly(true);
                }

                //actualInstoreMechanicId.setValue(record.data.PlanInstoreMechanicId);
                //actualInstoreMechanicTypeId.setValue(record.data.PlanInstoreMechanicTypeId);
                //actualInstoreMechanicDiscount.setValue(record.data.PlanInstoreMechanicDiscount);

                planPromoUpliftPercent.setValue(record.data.PlanPromoUpliftPercent);
                promoUpliftLockedUpdateCheckbox.setValue(!record.data.NeedRecountUplift);

                planPromoBaselineLSV.setValue(record.data.PlanPromoBaselineLSV);
                planPromoIncrementalLSV.setValue(record.data.PlanPromoIncrementalLSV);
                planPromoLSV.setValue(record.data.PlanPromoLSV);
                planPostPromoEffect.setValue(record.data.PlanPostPromoEffect);

                actualPromoUpliftPercent.setValue(record.data.ActualPromoUpliftPercent);
                actualPromoBaselineLSV.setValue(record.data.ActualPromoBaselineLSV);
                actualPromoIncrementalLSV.setValue(record.data.ActualPromoIncrementalLSV);
                actualPromoLSV.setValue(record.data.ActualPromoLSV);
                factPostPromoEffect.setValue(record.data.FactPostPromoEffect);

                parentWidget.setLoading(false);

                // Кнопки для изменения состояний промо
                var promoActions = Ext.ComponentQuery.query('button[isPromoAction=true]');

                // Определяем доступные действия
                me.defineAllowedActions(promoeditorcustom, promoActions, record.data.PromoStatusSystemName);
                if (!promoeditorcustom.isVisible())
                    promoeditorcustom.show();

                //если производится расчет данного промо, то необходимо сделать соотвествующие визуальные изменения окна: 
                //цвет хедера меняется на красный, кнопка Редактировать - disable = true, появляется кнопка Show Log, заблокировать кнопки смены статусов
                if (calculating) {
                    var toolbar = promoeditorcustom.down('customtoptoolbar');
                    toolbar.items.items.forEach(function (item, i, arr) {
                        item.el.setStyle('backgroundColor', '#B53333');
                        if (item.xtype == 'button' && ['btn_publish', 'btn_undoPublish', 'btn_sendForApproval', 'btn_reject', 'btn_approve', 'btn_rejectPlan', 'btn_plan', 'btn_close'].indexOf(item.itemId) > -1) {
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
                    toolbar.down('#btn_showlog').show();
                    toolbar.down('#btn_showlog').promoId = record.data.Id;

                    me.createTaskCheckCalculation(promoeditorcustom);
                }
            }, //ajax /odata/PromoStatuss success -end
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                parentWidget.setLoading(false);
                App.Notify.pushError(l10n.ns('tpm', 'text').value('failedStatusLoad'));
            }
        });
    },

    saveModel: function (model, window, close, reloadPromo) {
        var grid = Ext.ComponentQuery.query('#promoGrid')[0];
        var me = this;
        var store = null;

        if (grid) {
            store = grid.down('directorygrid').getStore();
        }

        window.setLoading(l10n.ns('core').value('savingText'));

        // Response возвращается не полностью верный, mappings игнорируются
        model.save({
            success: function (response) {
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

                ////Блокирование изменения панели при создании при отложенном расчёте
                //if (wasCreating) {


                //me.createTaskCheckCalculation(window);
                //me.onPrintPromoLog(window, grid, close);               

                //}

                return response;
            },
            failure: function () {
                model.reject();
                window.setLoading(false);

                return null;
            }
        });
    },

    savePromo: function (button, close, reloadForm) {
        var window = button.up('promoeditorcustom');

        var isStep1Complete = !window.down('#btn_promo_step1').hasCls('notcompleted');
        var isStep2Complete = !window.down('#btn_promo_step2').hasCls('notcompleted');
        var isStep3Complete = !window.down('#btn_promo_step3').hasCls('notcompleted');
        var isStep4Complete = !window.down('#btn_promo_step4').hasCls('notcompleted');
        var isStep5Complete = !window.down('#btn_promo_step5').hasCls('notcompleted');
        var isStep6Complete = !window.down('#btn_promo_step6').hasCls('notcompleted');

        if (this.validatePromoModel(window) && isStep1Complete && isStep2Complete && isStep3Complete && isStep4Complete && isStep5Complete && isStep6Complete) {
            var record = this.getRecord(window);

            if (window.promoStatusSystemName === 'Draft') {
                this.setPromoTitle(window, window.promoName, window.promoStatusName);
            } else {
                window.promoName = this.getPromoName(window);
                this.setPromoTitle(window, window.promoName, window.promoStatusName);
            }

            var model = this.buildPromoModel(window, record);
            var needRecountUplift = Ext.ComponentQuery.query('[itemId=PromoUpliftLockedUpdateCheckbox]')[0];

            if (needRecountUplift.value === true) {
                model.data.NeedRecountUplift = false;
            } else {
                model.data.NeedRecountUplift = true;
            }

            this.saveModel(model, window, close, reloadForm);

            // Если есть открытый календарь - обновить его
            var calendarGrid = Ext.ComponentQuery.query('scheduler');
            if (calendarGrid.length > 0) {
                calendarGrid[0].resourceStore.load();
            }
        } else {
            App.Notify.pushInfo(l10n.ns('tpm', 'text').value('completeStepsValidate'));
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
                        case 'CustomerMarketing':
                            visible = !record.get('IsCustomerMarketingApproved');
                            break;

                        case 'DemandPlanning':
                            visible = record.get('IsCustomerMarketingApproved') && !record.get('IsDemandPlanningApproved');
                            break;

                        case 'DemandFinance':
                            visible = record.get('IsCustomerMarketingApproved') && record.get('IsDemandPlanningApproved') && !record.get('IsDemandFinanceApproved');
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
        var producttree = promoForm.down('producttree');
        var treegrid = producttree.down('basetreegrid');
        var promoMechanic = promoForm.down('panel').down('promomechanic');

        var marsMechanic = promoMechanic.down('searchcombobox[name=MarsMechanicId]').getRawValue();
        var marsMechanicType = promoMechanic.down('searchcombobox[name=MarsMechanicTypeId]').getRawValue();
        var marsMechanicDiscountField = promoMechanic.down('[name=MarsMechanicDiscount]');
        var marsMechanicDiscountValue = marsMechanicDiscountField.getValue();

        var promoName = (treegrid.BrandAbbreviation ? treegrid.BrandAbbreviation : '') + ' ' + (treegrid.TechnologyAbbreviation ? treegrid.TechnologyAbbreviation : '') + ' ' +
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

    onClientTreeCheckChange: function (item, checked, eOpts) {
        var clientButton = Ext.ComponentQuery.query('button[itemId=btn_promo_step1]')[0],
            periodButton = Ext.ComponentQuery.query('button[itemId=btn_promo_step4]')[0],
            treegrid = clientButton.up('promoeditorcustom').down('clienttree').down('basetreegrid'),
            nodes = treegrid.getRootNode().childNodes,
            c = this.getController('tpm.client.ClientTree'),
            promoeditorcustom = Ext.ComponentQuery.query('promoeditorcustom')[0],
            promoperiod = promoeditorcustom.down('promoperiod'),
            dispatchStartDate = promoperiod.down('[name=DispatchStartDate]'),
            dispatchEndDate = promoperiod.down('[name=DispatchEndDate]'),
            durationStartDate = Ext.ComponentQuery.query('[name=DurationStartDate]')[0],
            durationEndDate = Ext.ComponentQuery.query('[name=DurationEndDate]')[0];

        var record = item;

        var calculation = Ext.ComponentQuery.query('promocalculation')[0];
        var activity = Ext.ComponentQuery.query('promoactivity')[0];

        if (calculation && activity) {
            var planPostPromoEffectFromCalculation = calculation.down('trigger[name=PlanPostPromoEffect]');


            var changePromoButton = Ext.ComponentQuery.query('#changePromo')[0];
            var model = this.getRecord(Ext.ComponentQuery.query('promoeditorcustom')[0]);

            model.data.PlanPostPromoEffectW1 = record.data.PostPromoEffectW1 || 0;
            model.data.PlanPostPromoEffectW2 = record.data.PostPromoEffectW2 || 0;
            model.data.PlanPostPromoEffect = (record.data.PostPromoEffectW1 + record.data.PostPromoEffectW2) || 0;

            if (changePromoButton) {
                if (changePromoButton.hidden === true) {
                    planPostPromoEffectFromCalculation.setValue(model.data.PlanPostPromoEffect);

                }
            }
        }

        if (durationStartDate.getValue() && durationEndDate.getValue()) {
            var clienttreegrid = promoeditorcustom.down('clienttreegrid'),
                //selectionModel = promoeditorcustom.down('clienttreegrid').getSelectionModel(),
                isBeforeStart = record.get('IsBeforeStart'),
                daysStart = record.get('DaysStart'),
                isDaysStart = record.get('IsDaysStart'),
                daysForDispatchStart = record.get('DaysStart'),
                isBeforeEnd = record.get('IsBeforeEnd'),
                daysEnd = record.get('DaysEnd'),
                isDaysEnd = record.get('IsDaysEnd'),
                daysForDispatchEnd = record.get('DaysEnd'),
                checkedClient = clienttreegrid.getView().getChecked();

            if (checkedClient.length != 0 && selectionModel.hasSelection() && durationStartDate.getValue() && durationEndDate.getValue()) {
                dispatchStartDate.reset();
                dispatchEndDate.reset();

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

        c.setCheckTree(nodes, false);

        if (checked) {
            item.set('checked', true);

            var store = treegrid.getStore(),
                parent = store.getNodeById(item.get('parentId')),
                buttonText = [],
                index = 0,
                text = '<b>' + l10n.ns('tpm', 'promoStap').value('basicStep1') + '</b><br><p>',
                displayHierarchy = '',
                innerHierarchy = '';

            buttonText[index++] = item.get('Name');
            if (parent) {
                while (parent.data.root !== true) {
                    buttonText[index++] = parent.get('Name');
                    parent.set('checked', true);

                    parent = store.getNodeById(parent.get('parentId'));
                }
            }

            for (var i = buttonText.length - 1; i >= 0; i--) {
                if (i !== 0) {
                    displayHierarchy += buttonText[i] + '<span style="font-size: 13px; font-family: MaterialDesignIcons; color: #A7AFB7"> </span>';
                    innerHierarchy += buttonText[i] + ' > ';
                } else {
                    displayHierarchy += buttonText[i];
                    innerHierarchy += buttonText[i];
                }
            }

            clientButton.up('promoeditorcustom').clientHierarchy = innerHierarchy;
            clientButton.up('promoeditorcustom').clientTreeId = item.get('ObjectId');
            clientButton.setText(text + displayHierarchy + '</p>');
            clientButton.hiddenText = innerHierarchy;
            clientButton.removeCls('notcompleted');
            clientButton.setGlyph(0xf133);
            clientButton.isComplete = true;
        } else {
            clientButton.up('promoeditorcustom').clientHierarchy = null;
            clientButton.up('promoeditorcustom').clientTreeId = null;
            clientButton.setText('<b>' + l10n.ns('tpm', 'promoStap').value('basicStep1') + '</b>');
            clientButton.addCls('notcompleted');
            clientButton.setGlyph(0xf130);
            clientButton.isComplete = false;

            dispatchStartDate.reset();
            dispatchEndDate.reset();

            var durationPeriod = durationStartDate.getValue() && durationEndDate.getValue() ? 'c ' + Ext.Date.format(durationStartDate.getValue(), "d.m.Y") + ' по ' + Ext.Date.format(durationEndDate.getValue(), "d.m.Y") : '',
                text = '<b>' + l10n.ns('tpm', 'promoStap').value('basicStep4') + '</b><br><p>Promo: ' + durationPeriod + '<br>Dispatch: ' + '</p>';

            var fieldSetDispatch = dispatchStartDate.up('fieldset');
            fieldSetDispatch.setTitle('Dispatch (0 days)');

            periodButton.setText(text);
            periodButton.addCls('notcompleted');
            periodButton.setGlyph(0xf130);
        }

        checkMainTab(clientButton.up(), clientButton.up('promoeditorcustom').down('button[itemId=btn_promo]'));
    },

    onProductTreeCheckChange: function (item, checked, eOpts) {
        var productButton = Ext.ComponentQuery.query('button[itemId=btn_promo_step2]')[0];
        var producttree = productButton.up('promoeditorcustom').down('producttree');
        var treegrid = producttree.down('basetreegrid'),
            nodes = treegrid.getRootNode().childNodes,
            c = this.getController('tpm.product.ProductTree');

        // если узел имеет тип subrange, то ищем отмеченные узлы на том же уровне        
        var multiCheck = false;
        if (productButton.up('promoeditorcustom').productTreeNodes === undefined) {
            productButton.up('promoeditorcustom').productTreeNodes = [];
        }

        if (item.get('Type').indexOf('Subrange') >= 0) {
            for (var i = 0; i < item.parentNode.childNodes.length && !multiCheck; i++) {
                var node = item.parentNode.childNodes[i];
                multiCheck = node !== item && node.get('checked') && node.get('Type').indexOf('Subrange') >= 0;
            }
        }
        // если нельзя отметить несколько, сбрасываем галочки
        if (!multiCheck) {
            c.setCheckTree(nodes, false);
            productButton.up('promoeditorcustom').productTreeNodes = [];
        }

        if (checked) {
            item.set('checked', true);

            if (productButton.up('promoeditorcustom').productTreeNodes.indexOf(item) < 0) {
                productButton.up('promoeditorcustom').productTreeNodes.push(item);
            }
        } else {
            var indexObjectId = productButton.up('promoeditorcustom').productTreeNodes.indexOf(item);
            if (indexObjectId >= 0) {
                productButton.up('promoeditorcustom').productTreeNodes.splice(indexObjectId, 1);
            }
        }

        var pathNames = '';
        productButton.up('promoeditorcustom').productTreeNodes.forEach(function (node) {
            pathNames += node.get('FullPathName') + ';';
        });
        productButton.up('promoeditorcustom').productHierarchy = pathNames.slice(0, -1);

        // устанавливаем параметры для валидации промо
        if (productButton.up('promoeditorcustom').productTreeNodes.length != 0) {
            var store = treegrid.getStore(),
                parent = store.getNodeById(item.get('parentId')),
                buttonText = [],
                index = 0,
                text = '<b>' + l10n.ns('tpm', 'promoStap').value('basicStep2') + '</b><br><p>',
                displayHierarchy = '';

            if (productButton.up('promoeditorcustom').productTreeNodes.length == 1) {
                buttonText[index++] = item.get('Name');

                // записываем сокращения Brand и Technology в переменные, чтобы потом использовать при составлении PromoName
                if (item.get('Type') === 'Brand') {
                    treegrid.BrandAbbreviation = item.get('Abbreviation');
                    productButton.up('promoeditorcustom').brandId = item.get('BrandId');
                }
                else if (item.get('Type') === 'Technology') {
                    treegrid.TechnologyAbbreviation = item.get('Abbreviation');
                    productButton.up('promoeditorcustom').technologyId = item.get('TechnologyId');
                }

                if (parent) {
                    while (parent.data.root !== true) {
                        buttonText[index++] = parent.get('Name');
                        parent.set('checked', true);

                        // записываем сокращения Brand и Technology в переменные, чтобы потом использовать при составлении PromoName
                        if (parent.get('Type') === 'Brand') {
                            treegrid.BrandAbbreviation = parent.get('Abbreviation');
                            productButton.up('promoeditorcustom').brandId = parent.get('BrandId');
                        }
                        else if (parent.get('Type') === 'Technology') {
                            treegrid.TechnologyAbbreviation = parent.get('Abbreviation');
                            productButton.up('promoeditorcustom').technologyId = parent.get('TechnologyId');
                        }

                        parent = store.getNodeById(parent.get('parentId'));
                    }
                }

                for (var i = buttonText.length - 1; i >= 0; i--) {
                    if (i !== 0) {
                        displayHierarchy += buttonText[i] + '<span style="font-size: 13px; font-family: MaterialDesignIcons; color: #A7AFB7"> </span>';
                    } else {
                        displayHierarchy += buttonText[i];
                    }
                }
            }
            else {
                displayHierarchy = '...';
            }

            productButton.setText(text + displayHierarchy + '</p>');
            productButton.removeCls('notcompleted');
            productButton.setGlyph(0xf133);
            productButton.isComplete = true;
        }
        else {
            productButton.setText('<b>' + l10n.ns('tpm', 'promoStap').value('basicStep2') + '</b>');
            productButton.addCls('notcompleted');
            productButton.setGlyph(0xf130);
            productButton.isComplete = false;
        }

        checkMainTab(productButton.up(), productButton.up('promoeditorcustom').down('button[itemId=btn_promo]'));

        //Если не начально загруженная запись - заблокировать кнопку productList
        if (productButton.up('promoeditorcustom').productsSetted) {
            var productListButton = producttree.down('#productList');
            productListButton.setDisabled(true);
        }
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
                } else {
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

                    promoController.setCompletedMechanicStep();
                } else {
                    promoController.setNotCompletedMechanicStep();
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

                promoController.setCompletedMechanicStep();
            } else {
                promoController.setNotCompletedMechanicStep();
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
                } else {
                    mechanicFields.instoreMechanicFields.instoreMechanicDiscount.reset();
                }
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
                    mechanicFields.actualInstoreMechanicFields.actualInstoreMechanicDiscount.reset();
                }
            }

            promoController.mechanicTypeChange(
                mechanicFields.actualInstoreMechanicFields.actualInstoreMechanicId,
                mechanicFields.actualInstoreMechanicFields.actualInstoreMechanicTypeId,
                mechanicFields.actualInstoreMechanicFields.actualInstoreMechanicDiscount,
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
                    mechanicFields.actualInstoreMechanicFields.actualInstoreMechanicDiscount.rawValue) {

                    if (mechanicFields.actualInstoreMechanicFields.actualInstoreMechanicTypeId.rawValue) {
                        mechanicFields.actualInstoreMechanicFields.actualInstoreMechanicDiscount.setValue(
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
                mechanicFields.actualInstoreMechanicFields.actualInstoreMechanicDiscount.rawValue) {
            }
        }
    },

    // Вызвать при изменении поля MechanicType.
    mechanicTypeChange: function (mechanicId, mechanicTypeId, mechanicDiscount, mechanicListForUnlockDiscountField, readOnly) {
        if (readOnly) {
            mechanicTypeId.setDisabled(true);
            mechanicDiscount.setDisabled(true);
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
            actualInstoreMechanicDiscount: promoMechanic.down('numberfield[name=ActualInstoreMechanicDiscount]')
        }
    },

    setNotCompletedMechanicStep: function () {
        var promoMechanicButton = Ext.ComponentQuery.query('button[itemId=btn_promo_step3]')[0];

        promoMechanicButton.addCls('notcompleted');
        promoMechanicButton.setGlyph(0xf130);
        promoMechanicButton.isComplete = false;
    },

    setCompletedMechanicStep: function () {
        var promoMechanicButton = Ext.ComponentQuery.query('button[itemId=btn_promo_step3]')[0];

        promoMechanicButton.removeCls('notcompleted');
        promoMechanicButton.setGlyph(0xf133);
        promoMechanicButton.isComplete = true;
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

        // ------------------------Trees-----------------------  
        var clientTreeGrid = promoeditorcustom.down('clienttreegrid');
        var productTreeGrid = promoeditorcustom.down('producttreegrid');

        var clientCrudAccess = ['Administrator', 'FunctionalExpert', 'CustomerMarketing', 'KeyAccountManager'];
        var productCrudAccess = ['Administrator', 'FunctionalExpert', 'CustomerMarketing', 'KeyAccountManager'];

        if (clientCrudAccess.indexOf(currentRole) === -1) {
            clientTreeGrid.setDisabled(true);
        }

        if (productCrudAccess.indexOf(currentRole) === -1) {
            productTreeGrid.setDisabled(true);
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
        }

        // Установка стиля для readOnly полей.
        var fieldsForReadOnlyCls = [
            'PlanPromoBaselineLSV', 'PlanPromoIncrementalLSV', 'PlanPromoLSV', 'PlanPostPromoEffect',
            'ActualPromoUpliftPercent', 'ActualPromoBaselineLSV', 'ActualPromoIncrementalLSV', 'ActualPromoLSV', 'FactPostPromoEffect',
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
            App.Notify.pushError(data.message);
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
        var clientTreeGrid = window.down('clienttreegrid');
        var clientTreeStore = clientTreeGrid.getStore();
        var productTreeGrid = window.down('producttreegrid');
        var productTreeStore = productTreeGrid.getStore();

        clientTreeStore.getRootNode().removeAll();
        clientTreeStore.getRootNode().setId('root');
        productTreeStore.getRootNode().removeAll();
        productTreeStore.getRootNode().setId('root');

        this.countLoadedComponents = 0;
        var readOnly = window.readOnly ? window.readOnly : false;
        this.fillPromoForm(window, record, readOnly, false, grid);
    },

    // метод вызывать СТРОГО ОДИН РАЗ ЗА ОДНО ОТКРЫТИЕ PROMO
    // привязать события загрузок
    bindAllLoadEvents: function (window, record, isCopy) {
        this.bindEventClientTreeLoad(window, record, isCopy);
        this.bindEventMarsMechanicLoad(window, record);
        this.bindEventPlanInstoreMechanicLoad(window, record);
        this.bindEventActualInstoreMechanicLoad(window, record);
        this.bindEventProductTreeLoad(window);
    },

    // если все компоненты загрузились, снимаем маску загрузки
    checkLoadingComponents: function () {
        if (this.countLoadedComponents < 2) {
            this.countLoadedComponents++;
        }

        if (this.countLoadedComponents == 2) {
            var window = Ext.ComponentQuery.query('promoeditorcustom');

            if (window.length > 0) {
                this.hideEditButtonForSomeRole();
                window[0].setLoading(false);
            }
        }
    },

    bindEventClientTreeLoad: function (window, record, isCopy) {
        var me = this;
        var client = window.down('container[name=promo_step1]');
        var clienttreegrid = client.down('clienttreegrid');
        var clienttreestore = clienttreegrid.store;

        // Если запись создаётся копированием, клиент берётся из календаря, а не из копируемой записи
        // Надо перепроверить логику
        var clientTreeId = null;

        if (record)
            clientTreeId = isCopy ? record.schedulerContext.resourceRecord.getId() : record.data.ClientTreeId;

        clienttreestore.on('load', function (store, node, records, success) {
            if (clientTreeId != null) {
                var c = me.getController('tpm.client.ClientTree');
                var clientData = null;
                var schedulerController = me.getController('tpm.schedule.SchedulerViewController');
                var dispatchStart = null;
                var dispatchEnd = null;
                var daysForDispatchStart = null;
                var daysForDispatchEnd = null;

                var period = window.down('container[name=promo_step4]');
                var dispatchStartDate = period.down('datefield[name=DispatchStartDate]');
                var dispatchEndDate = period.down('datefield[name=DispatchEndDate]');

                var targetnode = store.getNodeById(clientTreeId);

                if (targetnode) {
                    var selectionModel = clienttreegrid.getSelectionModel();
                    clientTreeId = null;

                    clientData = selectionModel.getSelection()[0] ? selectionModel.getSelection()[0] : targetnode;
                    c.updateTreeDetail(client.down('editorform'), clientData);

                    // period
                    var durationStartDate = period.down('datefield[name=DurationStartDate]');
                    var durationEndDate = period.down('datefield[name=DurationEndDate]');

                    /// Сомнительный участок
                    daysForDispatchStart = schedulerController.getDaysForDispatchDateFromClientSettings(
                        clientData.data.IsBeforeStart, clientData.data.DaysStart, clientData.data.IsDaysStart);

                    daysForDispatchEnd = schedulerController.getDaysForDispatchDateFromClientSettings(
                        clientData.data.IsBeforeEnd, clientData.data.DaysEnd, clientData.data.IsDaysEnd);

                    if (daysForDispatchStart !== null) {
                        dispatchStart = Ext.Date.add(durationStartDate.getValue(), Ext.Date.DAY,
                            schedulerController.getDaysForDispatchDateFromClientSettings(
                                clientData.data.IsBeforeStart, clientData.data.DaysStart, clientData.data.IsDaysStart));
                    }

                    if (daysForDispatchEnd !== null) {
                        dispatchEnd = Ext.Date.add(durationEndDate.getValue(), Ext.Date.DAY,
                            schedulerController.getDaysForDispatchDateFromClientSettings(
                                clientData.data.IsBeforeEnd, clientData.data.DaysEnd, clientData.data.IsDaysEnd));
                    }

                    if (isCopy) {
                        // Берутся настройки dispatch у клиента, на которого копируется промо в календаре
                        dispatchStartDate.setValue(dispatchStart);
                        dispatchEndDate.setValue(dispatchEnd);
                    } else {
                        // Берутся настройки dispatch у текущей записи, в том числе при открытии на редактирование и просмотр.
                        // Наличие или отсутсвие dispatch настроек у клиента не влияет на это действие.
                        dispatchStartDate.setValue(record.data.DispatchesStart);
                        dispatchEndDate.setValue(record.data.DispatchesEnd);
                    }
                }

                var promoPeriod = Ext.ComponentQuery.query('promoperiod')[0];
                var durationStartDate = promoPeriod.down('datefield[name=DurationStartDate]');
                var durationEndDate = promoPeriod.down('datefield[name=DurationEndDate]');

                var currentDate = new Date();
                currentDate.setHours(0, 0, 0, 0);

                //Установка завершенности при promo в статусе Closed, Finished, Started
                var statusName = client.up('promoeditorcustom').promoStatusName;
                if ((durationStartDate.getValue() < currentDate || durationEndDate.getValue() < durationStartDate.getValue()) &&
                    !(statusName == 'Closed' || statusName == 'Finished' || statusName == 'Started')) {
                    var promoPeriodButton = Ext.ComponentQuery.query('button[itemId=btn_promo_step4]')[0];

                    promoPeriodButton.addCls('notcompleted');
                    promoPeriodButton.setGlyph(0xf130);
                    promoPeriodButton.isComplete = false;
                }
            }

            me.checkLoadingComponents();
        });
    },

    bindEventProductTreeLoad: function (window) {
        var me = this;
        var product = window.down('container[name=promo_step2]');
        var productTreeGrid = product.down('producttreegrid');
        var productTreeStore = productTreeGrid.store;

        productTreeStore.on('load', function (store, node, records, success) {
            me.checkLoadingComponents();
        });
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
        var actualInstoreMechanicDiscount = mechanic.down('numberfield[name=ActualInstoreMechanicDiscount]');

        actualInstoreMechanicId.addListener('change', me.actualMechanicListener);
        actualInstoreMechanicTypeId.addListener('change', me.actualMechanicTypeListener);
        actualInstoreMechanicDiscount.addListener('change', me.actualMechanicDiscountListener);
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
                window.setLoading(false);
                App.Notify.pushError(data.statusText);
            }
        });
    },

    onPromoGridSelectionChange: function (selModel, selected) {
        this.onGridSelectionChange(selModel, selected);
    },

    onShowLogButtonClick: function (button) {
        if (button.promoId) {
            this.onPrintPromoLog(button.up('promoeditorcustom'));
        }
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
        var printPromoCalculatingWin = Ext.create('App.view.tpm.promo.PromoCalculatingWindow');

        printPromoCalculatingWin.show();
    },

    createTaskCheckCalculation: function (window) {
        var record = this.getRecord(window);
        var me = this;

        record.set('Calculating', true);

        var taskForLog = {
            run: function () {
                // Переодическое чтение лога в окно и проверка на окончание
                var params = 'promoId=' + record.get('Id');
                Ext.Ajax.request({
                    method: 'POST',
                    url: 'odata/Promoes/ReadPromoCalculatingLog?' + params,
                    success: function (response, opts) {
                        var result = Ext.JSON.decode(response.responseText);

                        // если промо закрыли, нужно прекратить запросы
                        if (!window.isVisible()) {
                            Ext.TaskManager.stop(taskForLog);
                        }
                        else {
                            var logWindow = Ext.ComponentQuery.query('promocalculatingwindow');
                            var textarea;

                            if (logWindow.length > 0) {
                                logWindow = logWindow[0];
                                textarea = logWindow.down('textarea');

                                var jobNameCreateString = "Запущен процесс расчета. \n",
                                    finCalculatingString = "Процесс расчета завершен.";

                                if (textarea) {
                                    if (textarea.getRawValue === "") {
                                        textarea.setValue(jobNameCreateString);
                                    }
                                    else {
                                        var logText = result.data;
                                        if (logText) {
                                            textarea.setValue(jobNameCreateString + logText);
                                        }
                                    }
                                }
                            }

                            //Проверка на закрытие
                            if (result.code == -1) { // ошибка во время проверки
                                Ext.TaskManager.stop(taskForLog);
                                Ext.Msg.alert("Внимание", result.opData);
                                me.unBlockPromo(window);
                            } else if (result.code == 1) { // задача завершилась
                                Ext.TaskManager.stop(taskForLog);
                                logWindow.jobEnded = true;
                                me.unBlockPromo(window);
                            }
                        }
                    },
                    failure: function () {
                        Ext.Msg.alert('ReadPromoCalculatingLog error');
                    }
                });
            },
            interval: (1000) * 0.5 // 0.5 секунды
        };

        Ext.TaskManager.start(taskForLog);
    },

    unBlockPromo: function (window) {
        var record = this.getRecord(window);
        var toolbar = window.down('customtoptoolbar');
        var me = this;

        if (toolbar) {
            toolbar.items.items.forEach(function (item, i, arr) {
                item.el.setStyle('backgroundColor', '#3f6895');
                if (item.xtype == 'button' && ['btn_publish', 'btn_undoPublish',
                    'btn_sendForApproval', 'btn_reject', 'btn_approve', 'btn_rejectPlan',
                    'btn_plan', 'btn_close'].indexOf(item.itemId) > -1) {
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

            toolbar.down('#btn_showlog').hide();
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

    // т.к. Форма редактирования кастомная, нет необходимости её обновлять
    updateDetailForm: function () { },

    // Показывать промо для которых возможно изменение статуса под текущей ролью
    onShowEditableButtonClick: function (button) {
        var store = this.getGridByButton(button).getStore(),
            proxy = store.getProxy()
        proxy.extraParams.canChangeStateOnly = !proxy.extraParams.canChangeStateOnly;
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

        window.down('[name=ActualInstoreMechanicDiscount]').setValue(record.data.ActualInStoreDiscount == 0 ? null : record.data.ActualInStoreDiscount);

        // Plan - Activity
        window.down('[name=PlanPromoUpliftPercent]').setValue(record.data.PlanPromoUpliftPercent);
        window.down('[name=PlanPromoBaselineLSV]').setValue(record.data.PlanPromoBaselineLSV);
        window.down('[name=PlanPromoIncrementalLSV]').setValue(record.data.PlanPromoIncrementalLSV);
        window.down('[name=PlanPromoLSV]').setValue(record.data.PlanPromoLSV);
        window.down('[name=PlanPostPromoEffect]').setValue(record.data.PlanPostPromoEffect);

        // Invoice & Prices
        window.down('[name=ActualInStoreShelfPrice]').setValue(record.data.ActualInStoreShelfPrice);
        window.down('[name=InvoiceNumber]').setValue(record.data.InvoiceNumber);

        // Actual - Activity
        window.down('[name=ActualPromoUpliftPercent]').setValue(record.data.ActualPromoUpliftPercent);
        window.down('[name=ActualPromoBaselineLSV]').setValue(record.data.ActualPromoBaselineLSV);
        window.down('[name=ActualPromoIncrementalLSV]').setValue(record.data.ActualPromoIncrementalLSV);
        window.down('[name=ActualPromoLSV]').setValue(record.data.ActualPromoLSV);
        window.down('[name=FactPostPromoEffect]').setValue(record.data.FactPostPromoEffect);
    },

    onPromoProductSubrangeDetailsWindowAfterRender: function (window) {
        var productTreeController = App.app.getController('tpm.product.ProductTree');
        var store = Ext.ComponentQuery.query('#promo_step2')[0].down('producttreegrid').getStore();

        store.addListener('load', function () {
            productTreeController.setCheckTree(store.getRootNode().childNodes, true);
            Ext.ComponentQuery.query('#promo_step2')[0].down('producttreegrid').unmask();
        });

        var productTreeGrid = Ext.widget('producttreegrid', {
            store: store,
            disabled: true,
            cls: 'template-tree scrollpanel promo-product-window'
        });

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
        var grid = button.up('combineddirectorypanel').down('directorygrid'),
            view = grid.getView(),
            offsetTop = $(view.getEl().dom).find('.jspVerticalBar').find('.jspDrag')[0].offsetTop;

        view.prevOffsetTop = offsetTop;
    }
});