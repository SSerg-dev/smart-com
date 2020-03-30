var refreshScroll = function refreshScroll(panel) {
    var targetEl = panel.getTargetEl();

    if (!targetEl) {
        return;
    }

    var domEl = $(targetEl.dom);

    if (!panel.hasScroll) {
        domEl.jScrollPane({
            verticalGutter: 35
        });

        domEl.bind(
            'jsp-initialised',
            function (event, isScrollable) {
                var jspData = domEl.data('jsp');

                if (jspData.getIsScrollableV()) {
                    domEl.addClass('has-vertical-scroll');
                } else {
                    domEl.removeClass('has-vertical-scroll');
                }

                if (jspData.getIsScrollableH()) {
                    domEl.addClass('has-horizontal-scroll');
                } else {
                    domEl.removeClass('has-horizontal-scroll');
                }
            }
        );

        domEl.bind(
            'jsp-scroll-x',
            function (event, scrollPositionX, isAtLeft, isAtRight) {
                if (!Ext.isNumber(scrollPositionX)) return;

                if (isExplorer()) {
                    var event = document.createEvent('WheelEvent', { "deltax": scrollPositionX, "deltamode": 0 });
                    event.initEvent('scroll', true, true)
                    targetEl.dom.dispatchEvent(event);
                } else {
                    targetEl.dom.dispatchEvent(new WheelEvent('scroll', {
                        deltaX: scrollPositionX,
                        deltaMode: 0
                    }));
                }
            }
        );

        domEl.bind(
            'jsp-scroll-y',
            function (event, scrollPositionY, isAtTop, isAtBottom) {
                var menuItems = panel.query('menuitem[hidden=false]');
                var shownMenuItems = [];
                for (var i = 0; i < menuItems.length; i++) {
                    if (menuItems[i].getTargetEl() != null) {
                        shownMenuItems.push(menuItems[i]);
                    }
                }

                if (isExplorer()) {
                    var event = document.createEvent('WheelEvent', { "deltay": 4, "deltamode": 0 });
                    event.initEvent('scroll', true, true)
                    targetEl.dom.dispatchEvent(event);
                }
                else {
                    for (var i = 0; i < shownMenuItems.length; i++) {
                        var mainPanel = shownMenuItems[i].up('combineddirectorypanel');
                        if (mainPanel != null) {
                            var headerHeight = mainPanel.down('header').getHeight();
                            var currentParentMenu = null;
                            var menuPanel = shownMenuItems[i].parentMenu;
                            if (menuPanel == currentParentMenu) continue;
                            if (menuPanel != null) {
                                menuPanel.setY(mainPanel.getY() + headerHeight);
                                menuPanel.el.setStyle('zIndex', 1);
                                menuPanel.el.shadow.zIndex = 1;
                                currentParentMenu = menuPanel;
                            }
                        }
                    }
                    if (Ext.isNumber(scrollPositionY)) {
                        targetEl.dom.dispatchEvent(new WheelEvent('scroll', {
                            deltaY: scrollPositionY,
                            deltaMode: 0
                        }));
                    }
                }
            }
        );

        panel.hasScroll = true;
    } else if (panel.hasScroll) {
        domEl.data('jsp').reinitialise();
    }
};

function checkMainTab(stepButtons, mainTab) {
    var status = true;

    for (i = 0; i < stepButtons.items.items.length; i++) {
        if (!stepButtons.items.items[i].isComplete) {
            status = false;
            break;
        }
    }

    var el = mainTab.getEl();

    if (el) {
        var stepCheck = el.dom.getElementsByClassName('custom-promo-main-step-check')[0];

        if (status) {
            stepCheck.innerText = '';
            stepCheck.style.color = '#3F6895';
        } else {
            stepCheck.innerText = '';
            stepCheck.style.color = '#ADBAC0';
        }
    }
};

var budgetsChangeListener = function (field, newValue, oldValue) {
    var container = field.up('container[buttonId]');
    var fields = Ext.ComponentQuery.query('#' + container.id + ' [name=planFieldSetBudgets] numberfield');
    var status = true;

    for (i = 0; i < fields.length; i++) {
        if ((fields[i].getValue() === null || !fields[i].isValid()) && fields[i].isChecked === true) {
            status = false;
            break;
        }
    }

    var panel = field.up('promoeditorcustom');
    var currentButton = panel.down('button[itemId=' + container.buttonId + ']');

    if (status) {
        currentButton.isComplete = status;
        currentButton.removeCls('notcompleted');
        currentButton.setGlyph(0xf133);
    } else {
        currentButton.isComplete = status;
        currentButton.addCls('notcompleted');
        currentButton.setGlyph(0xf130);
    }

    var mainTab = panel.down('button[itemId=btn_promoBudgets]');
    var stepButtons = currentButton.up('custompromotoolbar');

    checkMainTab(stepButtons, mainTab);
};



var activityChangeListener = function (field, newValue, oldValue) {
    var status = true;

    var panel = field.up('panel[name=promoActivity_step2]');
    var fields1 = panel.down('fieldset[name=activity]').items.items;
    var promoIsInOut = panel.up('promoeditorcustom').isInOutPromo;      // для InOut другая валидация

    for (i = 0; i < fields1.length; i++) {
        if (!(promoIsInOut && (fields1[i].name === 'ActualPromoUpliftPercent' || fields1[i].name === 'ActualPromoBaselineLSV'))) {
            if ((fields1[i].getValue() === null || !fields1[i].isValid()) && fields1[i].isChecked === true) {
                status = false;
                break;
            }
        }
    }

    var promoeditorcustom = field.up('promoeditorcustom');
    var currentButton = promoeditorcustom.down('button[itemId=btn_promoActivity_step2]');

    if (status) {
        currentButton.isComplete = status;
        currentButton.removeCls('notcompleted');
        currentButton.setGlyph(0xf133);
    } else {
        currentButton.isComplete = status;
        currentButton.addCls('notcompleted');
        currentButton.setGlyph(0xf130);
    }

    var mainTab = promoeditorcustom.down('button[itemId=btn_promoActivity]');
    var stepButtons = currentButton.up('custompromotoolbar');

    checkMainTab(stepButtons, mainTab);
};


// ToDo: Удалить
var calculationChangeListener = function (field, newValue, oldValue) {
    var container = field.up('container[buttonId]');
    var fields = Ext.ComponentQuery.query('#' + container.id + ' numberfield');
    var status = true;

    for (i = 0; i < fields.length; i++) {
        if (fields[i].getValue() === null || !fields[i].isValid()) {
            status = false;
            break;
        }
    }

    var panel = field.up('promoeditorcustom');
    var currentButton = panel.down('button[itemId=' + container.buttonId + ']');

    if (status) {
        currentButton.isComplete = status;
        currentButton.removeCls('notcompleted');
        currentButton.setGlyph(0xf133);
    } else {
        currentButton.isComplete = status;
        currentButton.addCls('notcompleted');
        currentButton.setGlyph(0xf130);
    }

    var mainTab = panel.down('button[itemId=btn_calculations]');
    var stepButtons = panel.down('panel[itemId=calculations]').down('custompromotoolbar');

    checkMainTab(stepButtons, mainTab);
};

// ToDo: Удалить
var calculationChangePostPromoEffectListener = function (field, newValue, oldValue) {
    postPromoEffectFields = getPostPromoEffectFields(field.up('promocalculation'));

    postPromoEffectFields.PlanPromoPostPromoEffectLSV.setValue(
        postPromoEffectFields.PlanPromoPostPromoEffectLSVW1.getValue() + postPromoEffectFields.PlanPromoPostPromoEffectLSVW2.getValue()
    );
};

var getPostPromoEffectFields = function (widgetName) {
    return {
        PlanPromoPostPromoEffectLSVW1: widgetName.down('numberfield[name=PlanPromoPostPromoEffectLSVW1]'),
        PlanPromoPostPromoEffectLSVW2: widgetName.down('numberfield[name=PlanPromoPostPromoEffectLSVW2]'),
        PlanPromoPostPromoEffectLSV: widgetName.down('numberfield[name=PlanPromoPostPromoEffectLSV]'),
        ActualPromoPostPromoEffectLSVW1: widgetName.down('numberfield[name=ActualPromoPostPromoEffectLSVW1]'),
        ActualPromoPostPromoEffectLSVW2: widgetName.down('numberfield[name=ActualPromoPostPromoEffectLSVW2]'),
        ActualPromoPostPromoEffectLSV: widgetName.down('numberfield[name=ActualPromoPostPromoEffectLSV]'),
    }
};

function setHighlightMargins(slider, value) {
    if (slider.highlightEl) {
        var range = (slider.maxValue || 100) - (slider.minValue || 0);

        var position1 = Math.abs((slider.minValue || 0) - slider.minValue);
        var position2 = range - Math.abs((slider.minValue || 0) - value);

        var percent1 = position1 > 0 ? 100.0 / range * position1 : 0;
        var percent2 = position2 > 0 ? 100.0 / range * position2 : 0;

        slider.highlightEl.setStyle({
            marginLeft: percent1 + '%',
            marginRight: percent2 + '%'
        });
    }
};

function renderWithDelimiter(value, delimFrom, delimTo) {
    return value.split(delimFrom).join('<span style="font-size: 13px; font-family: MaterialDesignIcons; color: #A7AFB7">' + delimTo + '</span>');
}

function dateDiff(str1, str2) {
    var diff = Date.parse(str2) - Date.parse(str1);
    return isNaN(diff) ? NaN : {
        diff: diff,
        ms: Math.ceil(diff % 1000),
        s: Math.ceil(diff / 1000 % 60),
        m: Math.ceil(diff / 60000 % 60),
        h: Math.ceil(diff / 3600000 % 24),
        d: Math.ceil(diff / 86400000)
    };
}

function rounder(value, toMln) {
    if (toMln) {
        value = value / 1000000;
    }
    return Math.round(value * 100) / 100;
}

//Обход параметров ядра для стилей

var bbStyleToOverrryde = {
    width: 60,
    minWidth: 60,
    maxWidth: 250,
    cls: 'custom-scheduler-toolbar',
    defaults: {
        ui: 'gray-button-toolbar',
        //padding: 6, //TODO: временно
        textAlign: 'center'
    },
    listeners: {
        beforerender: function (tb) {
            for (var i = 0; i < tb.items.items.length; i++) {
                var item = tb.items.items[i]
                if (item.itemId == 'table' || item.itemId == 'detail') {
                    item.setVisible(false);
                }
            }
        }
    }
};
Ext.override(App.view.core.toolbar.ReadonlyDeletedDirectoryToolbar, bbStyleToOverrryde);
Ext.override(App.view.core.toolbar.ReadonlyDirectoryToolbar, bbStyleToOverrryde);
Ext.override(App.view.core.toolbar.HardDeleteDirectoryToolbar, bbStyleToOverrryde);
Ext.override(App.view.core.toolbar.StandardDirectoryToolbar, bbStyleToOverrryde);
Ext.override(App.view.core.toolbar.ReadonlyWithoutFilterDirectoryToolbar, bbStyleToOverrryde);
Ext.override(App.view.core.toolbar.AddOnlyDirectoryToolbar, bbStyleToOverrryde);

Ext.override(App.view.core.security.RolesView, {
    header: {
        cls: 'roles-header'
    }
});

Ext.override(App.view.core.filter.ExtendedFilterWindow, {
    cls: 'scrollable custom-scheduler-toolbar',
    listeners: {
        beforerender: function (win) {
            // Изменение ширины тулбара
            var tb = win.down('#filtertoolbar');
            if (tb) {
                tb.width = 60;
                tb.minWidth = 60;
                tb.cls = 'custom-scheduler-toolbar';
                tb.defaults = {
                    ui: 'gray-button-toolbar',
                    //padding: 6, //TODO: временно,
                    textAlign: 'center'
                }
                for (var i = 0; i < tb.items.items.length; i++) {
                    var item = tb.items.items[i]
                    item.padding = null;
                    item.textAlign = 'center';
                }
            }
        }
    }
});

Ext.override(App.view.core.toolbar.ReportsFilterToolbar, {
    width: 60,
    minWidth: 60,
    maxWidth: 250,
    cls: 'custom-scheduler-toolbar',
    defaults: {
        ui: 'gray-button-toolbar',
        textAlign: 'center'
    },
    items: [{
        xtype: 'widthexpandbutton',
        ui: 'fill-gray-button-toolbar',
        text: l10n.ns('core', 'selectablePanelButtons').value('toolbarCollapse'),
        glyph: 0xf13d,
        glyph1: 0xf13e,
        target: function () {
            return this.up('toolbar');
        }
    }, {
        itemId: 'extfilterbutton',
        glyph: 0xf349,
        text: l10n.ns('core', 'toptoolbar').value('filterButtonText'),
        tooltip: l10n.ns('core', 'toptoolbar').value('filterButtonText')
    }, {
        itemId: 'downloadlogbutton',
        blocked: true,
        glyph: 0xf1da,
        text: l10n.ns('tpm', 'customtoptoolbar').value('downloadScheduleButtonText'),
        tooltip: l10n.ns('tpm', 'customtoptoolbar').value('downloadScheduleButtonText'),
        listeners: {
            click: function (button) {
                var me = this,
                    grid = button.up('combineddirectorypanel').down('directorygrid'),
                    selModel = grid.getSelectionModel(),
                    resource = button.resource || 'LoopHandlers',
                    action = button.action || 'Parameters';

                if (selModel.hasSelection()) {
                    grid.setLoading(true);
                    record = selModel.getSelection()[0],

                        breeze.EntityQuery
                            .from(resource)
                            .withParameters({
                                $actionName: action,
                                $method: 'POST',
                                $entity: record.getProxy().getBreezeEntityByRecord(record)
                            })
                            .using(Ext.ux.data.BreezeEntityManager.getEntityManager())
                            .execute()
                            .then(function (data) {
                                var resultData = data.httpResponse.data.value;
                                var result = JSON.parse(resultData);

                                var filePresent = result.OutcomingParameters != null &&
                                    result.OutcomingParameters.File != null &&
                                    result.OutcomingParameters.File.Value != null &&
                                    result.OutcomingParameters.File.Value.Name != null;
                                if (filePresent) {
                                    var href = document.location.href + Ext.String.format('/api/File/{0}?{1}={2}', 'ExportDownload', 'filename', result.OutcomingParameters.File.Value.Name);
                                    var aLink = document.createElement('a');
                                    aLink.download = "FileData";
                                    aLink.href = href;
                                    document.body.appendChild(aLink);
                                    aLink.click();
                                    document.body.removeChild(aLink);
                                } else {
                                    App.Notify.pushInfo(l10n.ns('tpm', 'customtoptoolbar').value('downloadError'));
                                }
                                grid.setLoading(false);
                            })
                            .fail(function (data) {
                                grid.setLoading(false);
                                App.Notify.pushError(me.getErrorMessage(data));
                            });
                } else {
                    console.log('No selection');
                    App.Notify.pushInfo(l10n.ns('tpm', 'customtoptoolbar').value('noSelectionError'));
                }
            }
        }
    }, '->', '-', {
        itemId: 'extfilterclearbutton',
        ui: 'blue-button-toolbar',
        disabled: true,
        glyph: 0xf232,
        text: l10n.ns('core', 'filter').value('filterEmptyStatus'),
        tooltip: l10n.ns('core', 'filter').value('filterEmptyStatus'),
        overCls: '',
        style: {
            'cursor': 'default'
        }
    }]
});

// Заменяет символ иерархии на '>' в treefsearchfield
Ext.override(App.view.tpm.nonenego.TreeFilterableSearchField, {
    onChange: function (searchfield) {
        if (searchfield) {
            var me = this;
            Ext.each(searchfield.values, function (val) {
                if (val.value.indexOf('  ') >= 0)
                    me.setValue(val.value.replace(//g, '>'));
            });
        };

        this.callParent();
    },
});

// Заменяет символ иерархии на '>' в textfield
Ext.override(Ext.form.field.Text, {
    onChange: function (value) {
        // проверяем по xtype потому, что datefield тоже срабатывает
        if (this.xtype == 'textfield' && value && value.indexOf('  ') >= 0) {
            this.setValue(value.replace(//g, '>'));
        };

        this.callParent();
    },
});

var calculationUpliftCheckbox = function (newValue) {
    var planUplift = Ext.ComponentQuery.query('[name=PlanPromoUpliftPercent]')[0];

    if (newValue.value === true) {
        planUplift.setReadOnly(false);
        planUplift.removeCls('readOnlyField');
    } else {
        planUplift.setReadOnly(true);
        planUplift.addCls('readOnlyField');
    }
}

// Дополнитеьная валидация для правильного отображения измеенённых полей
Ext.override(App.view.core.common.SearchComboBox, {
    afterSetValue: function (record) {
        if (this.needUpdateMappings) {
            this.updateMappingValues(record);
        }
        //Валидация всей формы после изменения
        var f = this.up('form');
        if (f && f.getForm) {
            var form = f.getForm();
            form.isValid();
        }
    }
});


// Чтобы делать отображение в всплывающих окнах
var editorChangeFunction = function (button) {
    var grid = this.getGridByButton(button),
        selModel = grid.getSelectionModel();

    if (!grid.editorModel || grid.editorModel.name != 'ToChangeEditorDetailWindowModel') {
        grid.editorModel = Ext.create('App.model.tpm.utils.ToChangeEditorDetailWindowModel', {
            grid: grid
        });
    }
    if (selModel.hasSelection()) {
        grid.editorModel.startDetailRecord(selModel.getSelection()[0]);
    } else {
        console.log('No selection');
    }
};

var updateEditorChangeFunction = function (button) {
    var grid = this.getGridByButton(button),
        selModel = grid.getSelectionModel();
    if (!grid.editorModel || grid.editorModel.name != 'ToChangeEditorDetailWindowModel') {
        grid.editorModel = Ext.create('App.model.tpm.utils.ToChangeEditorDetailWindowModel', {
            grid: grid
        });
    }
    if (selModel.hasSelection()) {
        grid.editorModel.startEditRecord(selModel.getSelection()[0]);
    } else {
        console.log('No selection');
    }
};

Ext.override(App.controller.core.role.Role, { switchToDetailForm: editorChangeFunction, onUpdateButtonClick: updateEditorChangeFunction });
Ext.override(App.controller.core.role.HistoricalRole, { switchToDetailForm: editorChangeFunction, onUpdateButtonClick: updateEditorChangeFunction });
Ext.override(App.controller.core.role.DeletedRole, { switchToDetailForm: editorChangeFunction, onUpdateButtonClick: updateEditorChangeFunction });

Ext.override(App.controller.core.associateduser.user.AdUser, { switchToDetailForm: editorChangeFunction, onUpdateButtonClick: updateEditorChangeFunction });
Ext.override(App.controller.core.associateduser.aduser.AssociatedAdUser, { switchToDetailForm: editorChangeFunction, onUpdateButtonClick: updateEditorChangeFunction });
Ext.override(App.controller.core.associateduser.userrole.AssociatedUserRole, { switchToDetailForm: editorChangeFunction, onUpdateButtonClick: updateEditorChangeFunction });
Ext.override(App.controller.core.associateduser.dbuser.DeletedAssociatedDbUser, { switchToDetailForm: editorChangeFunction, onUpdateButtonClick: updateEditorChangeFunction });
Ext.override(App.controller.core.associateduser.dbuser.HistoricalAssociatedDbUser, { switchToDetailForm: editorChangeFunction, onUpdateButtonClick: updateEditorChangeFunction });

Ext.override(App.controller.core.associatedconstraint.constraint.AssociatedConstraint, { switchToDetailForm: editorChangeFunction, onUpdateButtonClick: updateEditorChangeFunction });
Ext.override(App.controller.core.associatedconstraint.userrole.AssociatedUserRoleMain, { switchToDetailForm: editorChangeFunction, onUpdateButtonClick: updateEditorChangeFunction });

Ext.override(App.controller.core.setting.Setting, { switchToDetailForm: editorChangeFunction, onUpdateButtonClick: updateEditorChangeFunction });
Ext.override(App.controller.core.setting.HistoricalSetting, { switchToDetailForm: editorChangeFunction, onUpdateButtonClick: updateEditorChangeFunction });

Ext.override(App.controller.core.loophandler.AdminLoopHandler, { switchToDetailForm: editorChangeFunction, onUpdateButtonClick: updateEditorChangeFunction });
Ext.override(App.controller.core.loophandler.HistoricalLoopHandler, { switchToDetailForm: editorChangeFunction, onUpdateButtonClick: updateEditorChangeFunction });
Ext.override(App.controller.core.loophandler.LoopHandler, { switchToDetailForm: editorChangeFunction, onUpdateButtonClick: updateEditorChangeFunction });
Ext.override(App.controller.core.loophandler.UserLoopHandler, { switchToDetailForm: editorChangeFunction, onUpdateButtonClick: updateEditorChangeFunction });

Ext.override(App.controller.core.associatedaccesspoint.accesspoint.AssociatedAccessPoint, { switchToDetailForm: editorChangeFunction, onUpdateButtonClick: updateEditorChangeFunction });
Ext.override(App.controller.core.associatedaccesspoint.accesspoint.DeletedAssociatedAccessPoint, { switchToDetailForm: editorChangeFunction, onUpdateButtonClick: updateEditorChangeFunction });
Ext.override(App.controller.core.associatedaccesspoint.accesspoint.HistoricalAssociatedAccessPoint, { switchToDetailForm: editorChangeFunction, onUpdateButtonClick: updateEditorChangeFunction });

Ext.override(App.controller.core.associatedaccesspoint.accesspointrole.AssociatedAccessPointRole, { switchToDetailForm: editorChangeFunction, onUpdateButtonClick: updateEditorChangeFunction });

// Настройка почтовых уведомлений (верхний грид)
Ext.override(App.controller.core.associatedmailnotificationsetting.mailnotificationsetting.AssociatedMailNotificationSetting, { switchToDetailForm: editorChangeFunction, onUpdateButtonClick: updateEditorChangeFunction });
Ext.override(App.controller.core.associatedmailnotificationsetting.mailnotificationsetting.DeletedAssociatedMailNotificationSetting, { switchToDetailForm: editorChangeFunction, onUpdateButtonClick: updateEditorChangeFunction });
Ext.override(App.controller.core.associatedmailnotificationsetting.mailnotificationsetting.HistoricalAssociatedMailNotificationSetting, { switchToDetailForm: editorChangeFunction, onUpdateButtonClick: updateEditorChangeFunction });

// Настройка почтовых уведомлений (нижний грид)
Ext.override(App.controller.core.associatedmailnotificationsetting.recipient.AssociatedRecipient, { switchToDetailForm: editorChangeFunction });
Ext.override(App.controller.core.associatedmailnotificationsetting.recipient.HistoricalAssociatedRecipient, { switchToDetailForm: editorChangeFunction });


// custom Vtype
Ext.apply(Ext.form.field.VTypes, {
    eanNum: function (v) {
        return /^\d{0,}$/.test(v);
    },
    eanNumText: 'Must be a numeric EAN Case',
    eanNumMask: /\d{0,}/i
});

//Правка для IE при смене фокуса в гриде
Ext.override(Ext.view.Table, {
    focusRow: function (rowIdx) {
        var me = this,
            row,
            gridCollapsed = me.ownerCt && me.ownerCt.collapsed,
            record;

        if (me.isVisible(true) && !gridCollapsed && (row = me.getNode(rowIdx, true)) && me.el) {
            record = me.getRecord(row);
            rowIdx = me.indexInStore(row);

            me.selModel.setLastFocused(record);
            row.focus();
            me.focusedRow = row;
            me.fireEvent('rowfocus', rowIdx, true, Ext.isIE);

        }
    },
});

Ext.override(App.view.core.common.SearchComboBox, {
    onWindowClose: function () {
        if (this.defaultStoreState && this.getStore()) {
            this.getStore().applyState(this.defaultStoreState);
            this.getStore().extendedFilter.filter = null;
            this.getStore().load();
        }
    },
    onTrigger2Click: function () {
        var window = this.createWindow();

        if (window) {
            window.show();
            window.on('resize', function () { window.show() }, this);
            this.getStore().load();
        }
    },
});

// посчитать количество дней между датами (без границ)
var getDaysBetweenDates = function (date1, date2) {
    var date1WithoutHours = new Date(date1.getFullYear(), date1.getMonth(), date1.getDate());
    var date2WithoutHours = new Date(date2.getFullYear(), date2.getMonth(), date2.getDate());

    return (date1WithoutHours - date2WithoutHours) / 86400000;
};

// установить месяц в календаре (пикере) как в другом
var setMonthPicker = function (currentDateField, otherDateField) {
    // переопределяем, чтобы переключить месяц на тот, который выбран в start date
    var picker = currentDateField.getPicker();
    var otherDateValue = otherDateField.getValue();

    // если значение уже установлено, то переключение не требуется
    // а таекже проверяем установлено ли значение в start date
    if (currentDateField.getValue() !== null) {
        picker.setValue(currentDateField.getValue());
    }
    else if (otherDateValue !== null) {
        var dateMonth = new Date(otherDateValue.getFullYear(), otherDateValue.getMonth(), 1);
        picker.setValue(dateMonth);
    }
};

Ext.chart.LegendItem.override({
    updateSpecPosition: function (relativeTo) {
        var me = this,
            items = me.items,
            ln = items.length,
            item, i, x, y, translate, o,
            relativeX, relativeY;

        if (!relativeTo) {
            relativeTo = me.legend;
        }

        relativeX = relativeTo.x;
        relativeY = relativeTo.y;
        for (i = 0; i < ln; i++) {
            translate = true;
            item = items[i];
            switch (item.type) {
                case 'text':
                    x = relativeX + item.bbox.transform.height + 5;
                    y = relativeY;
                    translate = false;
                    break;
                case 'rect':
                    x = relativeX;
                    y = relativeY - item.width/2;
                    break;
                default:
                    x = relativeX;
                    y = relativeY;
            }

            o = {
                x: x,
                y: y
            };
            item.setAttributes(translate ? {
                translate: o
            } : o, true);
        }
    },

    //onMouseOver: function () {
    //    if (!this.legend.gaugeMouse) {
    //        this.callParent();
    //    }
    //},

    //onMouseOut: function () {
    //    if (!this.legend.gaugeMouse) {
    //        this.callParent();
    //    }
    //},

    onMouseDown: function () {
        if (!this.legend.gaugeMouse) {
            this.callParent();
        }
    }
});

Ext.chart.axis.Axis.override({
    drawAxis: function (init) {
        var me = this,
            i, 
            x = me.x,
            y = me.y,
            dashSize = me.dashSize,
            length = me.length,
            position = me.position,
            verticalAxis = (position == 'left' || position == 'right'),
            inflections = [],
            calcLabels = (me.isNumericAxis),
            stepCalcs = me.applyData(),
            step = stepCalcs.step,
            steps = stepCalcs.steps,
            stepsArray = Ext.isArray(steps),
            from = stepCalcs.from,
            to = stepCalcs.to,
            axisRange = (to - from) || 1,
            trueLength,
            currentX,
            currentY,
            path,
            subDashesX = me.minorTickSteps || 0,
            subDashesY = me.minorTickSteps || 0,
            dashesX = Math.max(subDashesX + 1, 0),
            dashesY = Math.max(subDashesY + 1, 0),
            dashDirection = (position == 'left' || position == 'top' ? -1 : 1),
            dashLength = dashSize * dashDirection,
            series = me.chart.series.items,
            firstSeries = series[0],
            gutters = firstSeries ? firstSeries.nullGutters : me.nullGutters,
            padding,
            subDashes,
            subDashValue,
            delta = 0,
            stepCount = 0,
            tick, axes, ln, val, begin, end;

        me.from = from;
        me.to = to;

        if (me.hidden || (from > to)) {
            return;
        }
        if ((stepsArray && (steps.length == 0)) || (!stepsArray && isNaN(step))) {
            return;
        }

        if (stepsArray) {
            steps = Ext.Array.filter(steps, function(elem, index, array) {
                return (+elem > +me.from && +elem < +me.to);
            }, this);

            steps = Ext.Array.union([me.from], steps, [me.to]);
        }
        else {
            steps = new Array;
            for (val = +me.from; val < +me.to; val += step) {
                steps.push(val);
            }
            steps.push(+me.to);
        }
        stepCount = steps.length;

        for (i = 0, ln = series.length; i < ln; i++) {
            if (series[i].seriesIsHidden) {
                continue;
            }
            if (!series[i].getAxesForXAndYFields) {
                continue;
            }
            axes = series[i].getAxesForXAndYFields();
            if (!axes.xAxis || !axes.yAxis || (axes.xAxis === position) || (axes.yAxis === position)) {
                gutters = series[i].getGutters();
                if ((gutters.verticalAxis !== undefined) && (gutters.verticalAxis != verticalAxis)) {
                    padding = series[i].getPadding();
                    if (verticalAxis) {
                        gutters = { lower: padding.bottom, upper: padding.top, verticalAxis: true };
                    } else {
                        gutters = { lower: padding.left, upper: padding.right, verticalAxis: false };
                    }
                }
                break;
            }
        }

        if (calcLabels) {
            me.labels = [];
        }
        if (gutters) {
            if (verticalAxis) {
                currentX = Math.floor(x);
                path = ["M", currentX + 0.5, y, "l", 0, -length];
                trueLength = length - (gutters.lower + gutters.upper);

                for (tick = 0; tick < stepCount; tick++) {
                    currentY = y - gutters.lower - (steps[tick] - steps[0]) * trueLength / axisRange;
                    path.push("M", currentX, Math.floor(currentY) + 0.5, "l", dashLength * 2, 0);

                    inflections.push([ currentX, Math.floor(currentY) ]);

                    if (calcLabels) {
                        me.labels.push(steps[tick]);
                    }
                }
            } else {
                currentY = Math.floor(y);
                path = ["M", x, currentY + 0.5, "l", length, 0];
                trueLength = length - (gutters.lower + gutters.upper);

                for (tick = 0; tick < stepCount; tick++) {
                    currentX = x + gutters.lower + (steps[tick] - steps[0]) * trueLength / axisRange;
                    path.push("M", Math.floor(currentX) + 0.5, currentY, "l", 0, dashLength * 2 + 1);

                    inflections.push([ Math.floor(currentX), currentY ]);

                    if (calcLabels) {
                        me.labels.push(steps[tick]);
                    }
                }
            }
        }

        subDashes = (verticalAxis ? subDashesY : subDashesX);
        if (Ext.isArray(subDashes)) {
            if (subDashes.length == 2) {
                subDashValue = +Ext.Date.add(new Date(), subDashes[0], subDashes[1]) - Date.now();
            } else {
                subDashValue = subDashes[0];
            }
        }
        else {
            if (Ext.isNumber(subDashes) && subDashes > 0) {
                subDashValue = step / (subDashes + 1);
            }
        }
        if (gutters && subDashValue) {
            for (tick = 0; tick < stepCount - 1; tick++) {
                begin = +steps[tick];
                end = +steps[tick+1];
                if (verticalAxis) {
                    for (value = begin + subDashValue; value < end; value += subDashValue) {
                        currentY = y - gutters.lower - (value - steps[0]) * trueLength / axisRange;
                        path.push("M", currentX, Math.floor(currentY) + 0.5, "l", dashLength, 0);
                    }
                }
                else {
                    for (value = begin + subDashValue; value < end; value += subDashValue) {
                        currentX = x + gutters.upper + (value - steps[0]) * trueLength / axisRange;
                        path.push("M", Math.floor(currentX) + 0.5, currentY, "l", 0, dashLength + 1);
                    }
                }
            }            
        }
        // Render
        if (!me.axis) {
            me.axis = me.chart.surface.add(Ext.apply({
                type: 'path',
                path: path
            }, me.axisStyle));
        }
        var blockRender = me.axis.blockRender ? true : false;
        me.axis.setAttributes({
            path: path
        }, blockRender);
        me.inflections = inflections;
        if (!init && me.grid) {
            me.drawGrid();
        }
        me.axisBBox = me.axis.getBBox();
        if (blockRender) {
            me.drawLabel();
        }
    },
});

Ext.chart.Legend.override({
    updatePosition: function () {
        var me = this;
        if (!me.customLegend) {
            this.callParent();
        } else {
            var items = me.items,
                itemsInOneRow = me.itemsInOneRow,
                pos, j, i, l, bbox, startX, itemHSpacing, itemWSpacing;
            if (me.isDisplayed()) {
                pos = me.calcPosition();
                me.x = pos.x;
                me.y = pos.y;
                itemHSpacing = me.itemHSpacing != undefined ? me.itemHSpacing : 0;
                itemWSpacing = me.itemWSpacing != undefined ? me.itemWSpacing : 0;
                startX = (me.startX != undefined ? me.startX : me.x) + me.padding;
                var posX = startX,
                    posY = me.y + itemHSpacing + me.padding,
                    itemHeight = items[0].getBBox().height;

                bbox = me.getBBox();
                for (j = 0; j < me.legendRow; j++) {
                    i = itemsInOneRow * j;
                    l = (j == me.legendRow - 1) ? items.length : itemsInOneRow + i;
                    for (i; i < l; i++) {
                        items[i].updateSpecPosition({ x: posX, y: posY });
                        posX += me.maxItemWidth + itemWSpacing;
                    }
                    posY = posY + itemHeight + itemHSpacing;
                    posX = startX;
                }
                if (isNaN(bbox.width) || isNaN(bbox.height)) {
                    if (me.boxSprite) {
                        me.boxSprite.hide(true);
                    }
                }
                else {
                    if (!me.boxSprite) {
                        me.createBox();
                    }
                    me.boxSprite.setAttributes(bbox, true);
                    me.boxSprite.show(true);
                }
            }

            if (me.displayValue) {
                var surfaceItems = me.chart.surface.items.items.slice();
                surfaceItems.forEach(function (item) {
                    if (item.legend) {
                        me.chart.surface.remove(item);
                    }
                });
                me.chart.legend.items.forEach(function (item, index) {
                    var textSprite = me.chart.surface.add({
                        type: 'text',
                        fill: '#0000A0',
                        text: me.chart.store.data.items[index].raw.value + '%',
                        x: item.items[1].attr.translation.x,
                        y: item.items[1].attr.translation.y,
                        font: me.valueFont,
                        zIndex: 101,
                        legend: true,
                    }).show(true);
                    textSprite.setAttributes({
                        translate: {
                            y: +textSprite.getBBox().height + me.valueHSpacing,
                        }
                    }, true);
                });
            }
        }
    },

    updateItemDimensions: function () {
        var me = this;

        if (!me.customLegend) {
            dim = this.callParent();
            return dim;
        } else {
            var items = me.items,
                itemHSpacing,
                itemWSpacing,
                resMaxWidth = 0,
                resMaxHeight = 0,
                resTotalWidth = 0,
                resTotalHeight = 0,
                itemWidth;

            itemHSpacing = me.itemHSpacing != undefined ? me.itemHSpacing : 0;
            itemWSpacing = me.itemWSpacing != undefined ? me.itemWSpacing : 0;

            me.maxItemWidth = 0;
            for (i = 0; i < items.length; i++) {
                items[i].addCls('chart-legend');
                itemWidth = items[i].getBBox().width + itemWSpacing
                if (me.maxItemWidth < itemWidth) {
                    me.maxItemWidth = itemWidth;
                }
            };

            me.itemsInOneRow = Math.floor(me.chart.surface.width / me.maxItemWidth);
            me.legendRow = Math.ceil(items.length / me.itemsInOneRow);

            resTotalWidth = (me.legendRow == 1 ? items.length : me.itemsInOneRow) * me.maxItemWidth;
            resMaxWidth = resTotalWidth;

            resTotalHeight = (items[0].getBBox().height) * me.legendRow + itemHSpacing * (me.legendRow-1);
            resMaxHeight = resTotalHeight;

            return {
                totalWidth: resTotalWidth,
                totalHeight: resTotalHeight,
                maxWidth: resMaxWidth,
                maxHeight: resMaxHeight
            };
        }
    },

    getBBox: function () {
        var me = this,
            surface = me.chart.surface,
            width = (me.width < surface.width) ? me.width : surface.width
        return {
            x: Math.round(me.x) - me.boxStrokeWidth / 2,
            y: Math.round(me.y) - me.boxStrokeWidth / 2,
            width: width + me.boxStrokeWidth,
            height: me.height + me.boxStrokeWidth
        };
    },
});

Ext.chart.series.Bar.override({
    isItemInPoint: function(x, y, item) {
        if (!item.sprite) {
            return false;
        }
        var bbox = item.sprite.getBBox();
        return bbox.x <= x && bbox.y <= y
            && (bbox.x + bbox.width) >= x
            && (bbox.y + bbox.height) >= y;
    },
});

//Кастомные конфиги: custom, strokeColor
Ext.chart.series.Gauge.override({

    initialize: function () {
        var me = this;
        if (!me.custom) {
            this.callParent();
        } else {
            var store = me.chart.getChartStore(),
                data = store.data.items,
                label = me.label,
                ln = data.length;

            me.yField = [];
            if (label && label.field && ln > 0) {
                for (var i = 0; i < ln; i++) {
                    me.yField.push(data[i].get(label.field));
                }
            }
        }
    },

    drawSeries: function () {
        var me = this,
            chart = me.chart,
            store = chart.getChartStore();
        if (!me.custom) {
            this.callParent();
        } else {
            var group = me.group,
                animate = me.chart.animate,
                axis = me.chart.axes.get(0),
                minimum = axis && axis.minimum || me.minimum || 0,
                maximum = axis && axis.maximum || me.maximum || 0,
                field = me.angleField || me.field || me.xField,
                surface = chart.surface,
                chartBBox = chart.chartBBox,
                donut = +me.donut,
                values = [],
                showNegative = me.chart.showNegative,
                startRho,
                rhoOffset,
                items = [],
                seriesStyle = me.seriesStyle,
                colorArrayStyle = me.colorArrayStyle,
                colorArrayLength = colorArrayStyle && colorArrayStyle.length || 0,
                cos = Math.cos,
                sin = Math.sin,
                defaultStart = -180,
                rendererAttributes, centerX, centerY, slice, slices, sprite,
                item, ln, i,
                spriteOptions, splitAngle1, splitAngle2, sliceA, sliceB, sliceC;

            Ext.apply(seriesStyle, me.style || {});

            me.setBBox();
            bbox = me.bbox;


            if (me.colorSet) {
                colorArrayStyle = me.colorSet;
                colorArrayLength = colorArrayStyle.length;
            }
            colorArrayStyle = colorArrayStyle.slice().reverse();

            if (!store || !store.getCount() || me.seriesIsHidden) {
                me.hide();
                me.items = [];
                return;
            }

            centerX = me.centerX = chartBBox.x + (chartBBox.width / 2);
            centerY = me.centerY = chartBBox.y + chartBBox.height;
            me.radius = Math.min(centerX - chartBBox.x, centerY - chartBBox.y);
            me.slices = slices = [];
            me.items = items = [];

            for (i = 0; i < store.getCount(); i++) {
                if (!values[i]) {
                    if (showNegative) {
                        values[i] = store.getAt(i).get(field);
                    } else {
                        values[i] = store.getAt(i).get(field) >= 0 ? store.getAt(i).get(field) : 0;
                    }
                }
            }

            startRho = me.radius * +donut / 100;
            rhoOffset = (me.radius - startRho) / values.length - 0.15;
            
            sliceDefault = {
                series: me,
                value: maximum,
                startAngle: defaultStart,
                endAngle: 0,
                rho: me.radius,
                startRho: startRho,
                endRho: me.radius
            };

            if (values.length == 3) {
                minimum = Math.min(values[0], values[1], values[2]);
                splitAngle1 = defaultStart * (1 - (values[0] - minimum) / (maximum - minimum));
                splitAngle2 = defaultStart * (1 - (values[1] - minimum) / (maximum - minimum));
                splitAngle3 = defaultStart * (1 - (values[2] - minimum) / (maximum - minimum));

                slice1 = {
                    series: me,
                    value: values[0],
                    startAngle: defaultStart,
                    endAngle: splitAngle1,
                    rho: me.radius,
                    startRho: startRho,
                    endRho: me.radius - rhoOffset * 2
                };
                slice2 = {
                    series: me,
                    value: values[1],
                    startAngle: defaultStart,
                    endAngle: splitAngle2,
                    rho: me.radius,
                    startRho: startRho + rhoOffset,
                    endRho: me.radius - rhoOffset
                };
                slice3 = {
                    series: me,
                    value: values[2],
                    startAngle: defaultStart,
                    endAngle: splitAngle3,
                    rho: me.radius,
                    startRho: startRho + rhoOffset * 2,
                    endRho: me.radius
                };
                slices.push(sliceDefault, slice3, slice2, slice1);
            } else {
                splitAngle1 = defaultStart * (1 - (values[0] - minimum) / (maximum - minimum));
                splitAngle2 = defaultStart * (1 - (values[1] - minimum) / (maximum - minimum));

                slice1 = {
                    series: me,
                    value: values[1],
                    startAngle: defaultStart,
                    endAngle: splitAngle2,
                    rho: me.radius,
                    startRho: startRho,
                    endRho: me.radius - rhoOffset
                };
                slice2 = {
                    series: me,
                    value: values[0],
                    startAngle: defaultStart,
                    endAngle: splitAngle1,
                    rho: me.radius,
                    startRho: startRho + rhoOffset,
                    endRho: me.radius
                };
                slices.push(sliceDefault, slice1, slice2);
            }

            for (i = 0, ln = slices.length; i < ln; i++) {
                slice = slices[i];
                sprite = group.getAt(i);

                rendererAttributes = Ext.apply({
                    segment: {
                        startAngle: slice.startAngle,
                        endAngle: slice.endAngle,
                        margin: 0,
                        rho: slice.rho,
                        startRho: slice.startRho,
                        endRho: slice.endRho
                    }
                }, Ext.apply(seriesStyle, colorArrayStyle && { fill: colorArrayStyle[i % colorArrayLength] } || {}));

                item = Ext.apply({},
                    rendererAttributes.segment, {
                        slice: slice,
                        series: me,
                        storeItem: store.getAt(i),
                        index: i
                    });
                items[i] = item;

                if (!sprite) {
                    spriteOptions = Ext.apply({
                        type: "path",
                        group: group
                    }, Ext.apply(seriesStyle, colorArrayStyle && { fill: colorArrayStyle[i % colorArrayLength] } || {}));
                    sprite = surface.add(Ext.apply(spriteOptions, rendererAttributes));
                }
                slice.sprite = slice.sprite || [];
                item.sprite = sprite;
                slice.sprite.push(sprite);
                if (animate) {
                    rendererAttributes = me.renderer(sprite, store.getAt(i), rendererAttributes, i, store);
                    sprite._to = rendererAttributes;
                    me.onAnimate(sprite, {
                        to: rendererAttributes
                    });
                } else {
                    rendererAttributes = me.renderer(sprite, store.getAt(i), Ext.apply(rendererAttributes, {
                        hidden: false
                    }), i, store);
                    sprite.setAttributes(rendererAttributes, true);
                }
            }
            if (me.needle) {
                splitAngle = splitAngle * Math.PI / 180;
                var strokeColor = '#222';
                if (me.strokeColor) strokeColor = me.strokeColor;
                if (!me.needleSprite) {
                    me.needleSprite = me.chart.surface.add({
                        type: 'path',
                        path: ['M', centerX + (me.radius * +donut / 100) * cos(splitAngle),
                            centerY + -Math.abs((me.radius * +donut / 100) * sin(splitAngle)),
                            'L', centerX + me.radius * cos(splitAngle),
                            centerY + -Math.abs(me.radius * sin(splitAngle))],
                        'stroke-width': 4,
                        'stroke': strokeColor,
                    });
                } else {
                    if (animate) {
                        me.onAnimate(me.needleSprite, {
                            to: {
                                path: ['M', centerX + (me.radius * +donut / 100) * cos(splitAngle),
                                    centerY + -Math.abs((me.radius * +donut / 100) * sin(splitAngle)),
                                    'L', centerX + me.radius * cos(splitAngle),
                                    centerY + -Math.abs(me.radius * sin(splitAngle))]
                            }
                        });
                    } else {
                        me.needleSprite.setAttributes({
                            type: 'path',
                            path: ['M', centerX + (me.radius * +donut / 100) * cos(splitAngle),
                                centerY + -Math.abs((me.radius * +donut / 100) * sin(splitAngle)),
                                'L', centerX + me.radius * cos(splitAngle),
                                centerY + -Math.abs(me.radius * sin(splitAngle))]
                        });
                    }
                }
                me.needleSprite.setAttributes({
                    hidden: false
                }, true);
            }

            for (i = 0; i < store.getCount(); i++) {
                delete values[i];
            }
        }
    },
});

Ext.define('Override.RowExpander', {
    override: 'Ext.grid.plugin.RowExpander',
    toggleRow: function (rowIdx, record) {
        var me = this,
            view = me.view,
            rowNode = view.getNode(rowIdx),
            row = Ext.fly(rowNode, '_rowExpander'),
            nextBd = row.down(me.rowBodyTrSelector, true),
            isCollapsed = row.hasCls(me.rowCollapsedCls),
            addOrRemoveCls = isCollapsed ? 'removeCls' : 'addCls',
            ownerLock, rowHeight, fireView;

        // Suspend layouts because of possible TWO views having their height change
        Ext.suspendLayouts();
        row[addOrRemoveCls](me.rowCollapsedCls);
        Ext.fly(nextBd)[addOrRemoveCls](me.rowBodyHiddenCls);
        me.recordsExpanded[record.internalId] = isCollapsed;
        view.refreshSize();

        // Sync the height and class of the row on the locked side
        if (me.grid.ownerLockable) {
            ownerLock = me.grid.ownerLockable;
            fireView = ownerLock.getView();
            view = ownerLock.lockedGrid.view;
            rowHeight = row.getHeight();
            row = Ext.fly(view.getNode(rowIdx), '_rowExpander');
            row.setHeight(rowHeight);
            row[addOrRemoveCls](me.rowCollapsedCls);
            view.refreshSize();
        } else {
            fireView = view;
        }
        fireView.fireEvent(isCollapsed ? 'expandbody' : 'collapsebody', row.dom, record, nextBd);
        // Coalesce laying out due to view size changes
        Ext.resumeLayouts(true);
    },
});

Ext.chart.series.Bar.override({
    drawSeries: function () {
        this.getPaths();

        var items = this.items,
            ln = items.length,
            isNull = true;
        for (i = 0; i < ln; i++) {
            if (items[i].value[1] != 0) {
                isNull = false;
            }
        }
        if (!isNull) {
            this.callParent();
        }
    }
});


Ext.override(Sch.view.SchedulerGridView, {
    onEventSelect: function (recordToSelect) {
        if (recordToSelect.store.lastSelected) {
            this.onEventDeselect(recordToSelect.store.lastSelected);
        };

        var b = this.getEventNodesByRecord(recordToSelect);
        if (b) {
            b.addCls(this.selectedEventCls)
        }
        recordToSelect.store.lastSelected = recordToSelect;
    },
});

Ext.override(Sch.feature.ResizeZone, {
    getStartEndDates: function () {
        var e = this.resizer,
            c = e.el,
            d = this.schedulerView,
            b = e.isStart,
            g, a, f;
        if (b) {
            if (d.getMode() === "horizontal") {
                f = [d.rtl ? c.getRight() : c.getLeft() + 1, c.getTop()]
            } else {
                f = [(c.getRight() + c.getLeft()) / 2, c.getTop()]
            }
            a = e.eventRecord.getEndDate();
            if (d.snapRelativeToEventStartDate) {
                g = d.getDateFromXY(f);
                g = d.timeAxis.roundDate(g, e.eventRecord.getStartDate())
            } else {
                g = d.getDateFromXY(f, "round")
            }
        } else {
            if (d.getMode() === "horizontal") {
                f = [d.rtl ? c.getLeft() : c.getRight(), c.getBottom()]
            } else {
                f = [(c.getRight() + c.getLeft()) / 2, c.getBottom()]
            }
            g = e.eventRecord.getStartDate();
            if (d.snapRelativeToEventStartDate) {
                a = d.getDateFromXY(f);
                a = d.timeAxis.roundDate(a, e.eventRecord.getEndDate())
            } else {
                a = d.getDateFromXY(f, "round");
                //Удаляем одну секунд для корректного расчета даты
                a.setTime(a.getTime() - 1000);
            }
        }
        g = g || e.start;
        a = a || e.end;
        if (e.dateConstraints) {
            g = Sch.util.Date.constrain(g, e.dateConstraints.start, e.dateConstraints.end);
            a = Sch.util.Date.constrain(a, e.dateConstraints.start, e.dateConstraints.end)
        }
        return {
            start: g,
            end: a
        }
    },
});

// проверка соединения signalR
function requestHub(func, args) {
    try {
        var isConnected = ($.connection.hub && $.connection.hub.state === $.signalR.connectionState.connected);
        if (isConnected) {
            func.apply(this, args);
        } else {
            $.connection.hub.start()
                .done(function () {
                    console.log('Connection hub is started');
                    func.apply(this, args);
                })
                .fail(function () { console.log('Connection hub is failed'); });
        }
    } catch (e) {
        console.log('Error while ' + func);
    }
};

Ext.override(Ext.selection.RowModel,
    {
        isRowSelected: function (record, index) {
            try {
                return this.isSelected(record);
            }
            catch (e) {
                return false;
            }
        }
    });

Ext.override(Ext.grid.plugin.BufferedRenderer, {
    getScrollHeight: function () {
        var me = this,
            view = me.view,
            store = me.store,
            doCalcHeight = !me.hasOwnProperty('rowHeight'),
            storeCount = me.store.getCount(),
            additionalBottomShift = 50, // добавка к высоте прокрутки, чтобы нижняя строка грида была полностью видна (даже с запасом)
            wrapHeight = 160; // высота раскрытой части строки (в данном случае это высота дашборда строки промо)

        if (!storeCount) {
            return 0;
        }
        if (doCalcHeight) {
            if (view.all.getCount()) {
                if (!me.cmp.hasExpandedRows || me.cmp.expandedRows === undefined || me.cmp.expandedRows === null) {
                    me.rowHeight = Math.floor(view.body.getHeight() / view.all.getCount());
                } else {
                    // для корректного вычисления высоты прокрутки при наличии раскрытых строк рассчетное значение высоты строки не подходит
                    me.rowHeight = 29;
                }
            }
        }

        if (me.cmp.hasExpandedRows) {
            if (me.cmp.expandedRows === undefined || me.cmp.expandedRows === null) {
                return this.scrollHeight = Math.floor((store.buffered ? store.getTotalCount() : store.getCount()) * me.rowHeight) + additionalBottomShift;
            } else {
                // для обновления прокрутки при сворачивании/разворачивании строки добавляем высоту раскрытых строк
                return this.scrollHeight = Math.floor((store.buffered ? store.getTotalCount() : store.getCount()) * me.rowHeight) + wrapHeight * me.cmp.expandedRows;
            }
        } else {
            return this.scrollHeight = Math.floor((store.buffered ? store.getTotalCount() : store.getCount()) * me.rowHeight) + additionalBottomShift;
        }
    }
});

Ext.override(App.controller.core.Main, {
    showUserDashboard: function (me) {
        var button = {
            widget: 'userdashboard',
            cloneConfig: function () {
                //Что бы ошибки в консоли не было
            }
        }
        me.onOpenViewButtonClick(button);
    }
});

Ext.override(App.menu.core.MenuManager, {
    init: function () {
        var role = App.UserInfo.getCurrentRole();
        if (role) {
            this.callParent(arguments);
            var controller = App.app.getController('core.Main');
            if (['DemandFinance', 'KeyAccountManager', 'DemandPlanning', 'CustomerMarketing', 'CMManager'].includes(role.SystemName)) {
                controller.showUserDashboard(controller);
            }
        } else {
            //Скрываем всё для неавторизованных пользователей
            Ext.ComponentQuery.query('#drawer')[0].hide();
            Ext.ComponentQuery.query('#header')[0].hide();
            Ext.ComponentQuery.query('#systempanel')[0].hide();
        }
    }
});

Ext.define('App.global.Statuses', {
    statics: {
        AllStatuses: ["Draft", "DraftPublished", "OnApproval", "Planned", "Approved", "Deleted", "Finished", "Cancelled", "Closed", "Started"],
        AllStatusesWithoutDraft: ["DraftPublished", "OnApproval", "Planned", "Approved", "Finished", "Cancelled", "Closed", "Started"],
        AllStatusesBeforeClosedWithoutDraft: ["DraftPublished", "OnApproval", "Planned", "Approved", "Finished", "Started"],
        AllStatusesBeforeStartedWithoutDraft: ["DraftPublished", "OnApproval", "Planned", "Approved"],
        Finished: ["Finished"],
    }
});

Ext.override(Ext.grid.plugin.HeaderResizer, {

    // get the region to constrain to, takes into account max and min col widths
    getConstrainRegion: function () {
        var me = this,
            dragHdEl = me.dragHd.el,
            rightAdjust = 0,
            nextHd,
            lockedGrid;

        // If forceFit, then right constraint is based upon not being able to force the next header
        // beyond the minColWidth. If there is no next header, then the header may not be expanded.
        if (me.headerCt.forceFit) {
            nextHd = me.dragHd.nextNode('gridcolumn:not([hidden]):not([isGroupHeader])');
            if (nextHd) {
                //Отличные исходники
                if (!me.headerInSameGrid(nextHd)) {
                    nextHd = null;
                    rightAdjust = 0;
                } else {
                    rightAdjust = nextHd.getWidth() - me.minColWidth;
                }
            }
        }

        // If resize header is in a locked grid, the maxWidth has to be 30px within the available locking grid's width
        else if ((lockedGrid = me.dragHd.up('tablepanel')).isLocked) {
            rightAdjust = me.dragHd.up('[scrollerOwner]').getWidth() - lockedGrid.getWidth() - 30;
        }

        // Else ue our default max width
        else {
            rightAdjust = me.maxColWidth - dragHdEl.getWidth();
        }

        return me.adjustConstrainRegion(
            dragHdEl.getRegion(),
            0,
            rightAdjust,
            0,
            me.minColWidth
        );
    },
});