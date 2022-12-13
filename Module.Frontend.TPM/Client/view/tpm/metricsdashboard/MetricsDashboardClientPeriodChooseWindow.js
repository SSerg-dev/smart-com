Ext.define('App.view.tpm.metricsdashboard.MetricsDashboardClientPeriodChooseWindow', {
    extend: 'App.view.core.common.EditorWindow',
    alias: 'widget.metricsdashboardclientperiodchoosewindow',

    cls: 'client-dashboard-client-year-choose-window',
    width: 500,
    minWidth: 500,

    defaults: {
        componentCls: 'client-dashboard-client-year-choose-window-item',
        labelAlign: 'top',
        baseBodyCls: 'client-dashboard-client-year-choose-window-field-label',
        labelSeparator: '',
        flex: 1,
    },

    items: [
        {
            xtype: 'treesearchfield',
            itemId: 'ClientTreeField',
            fieldLabel: l10n.ns('tpm', 'ClientDashboard').value('SelectClient'),
            trigger2Cls: '',
            selectorWidget: 'clienttree',
            valueField: 'Name',
            displayField: 'Name',
            allowBlank: false,
            editable: false,
            hideNotHierarchyBtns: true,
            store: {
                model: 'App.model.tpm.clienttree.ClientTree',
                autoLoad: false,
                root: {}
            },
            onSelectButtonClick: function () { },
            onSelectionChange: function () { },
            listeners: {
                beforerender: function (picker) {
                    var newOnTrigger1Click = function () {
                        var clientDashboardController = App.app.getController('tpm.clientdashboard.ClientDashboard');
                        clientDashboardController.onTrigger1Click(picker);
                    }
                    picker.onTrigger1Click = newOnTrigger1Click;
                }
            }
        },
        {
            xtype: 'custommarsdatefield',
            itemId: 'PeriodField',
            editable: false,
            fieldLabel: l10n.ns('tpm', 'ClientDashboard').value('SelectPeriod'),
        },
        {
            xtype: 'numberfield',
            itemId: 'YearField',
            fieldLabel: l10n.ns('tpm', 'ClientDashboard').value('SelectYear'),
            name: 'year',
            allowBlank: false,
            allowOnlyWhiteSpace: false,
            componentCls: 'client-dashboard-client-year-choose-window-item client-dashboard-client-year-choose-window-item-last',
            listeners: {
                added: function (field) {
                    var currentDate = new Date();
                    field.setValue(currentDate.getFullYear());
                }
            }
        }
    ],
    buttons: [
        {
            text: l10n.ns('core', 'buttons').value('cancel'),
            itemId: 'cancel'
        },
        {
            text: l10n.ns('core', 'buttons').value('ok'),
            itemId: 'choose',
            ui: 'green-button-footer-toolbar',
        }
    ]
})