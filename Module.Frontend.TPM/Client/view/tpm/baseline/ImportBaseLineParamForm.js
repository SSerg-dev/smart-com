Ext.define('App.view.tpm.baseline.ImportBaseLineParamForm', {
    extend: 'App.view.core.common.ImportParamForm',
    alias: 'widget.importforecastparamform',

    initFields: function (fieldValues) {
        //fieldValues = App.Util.getParamFieldValuesFromConstraints(fieldValues);
        //// StartMarsPeriod - начало следующего периода
        //var periodEndDate = (new App.MarsDate()).getPeriodEndDate();
        //var nextPeriodMarsDate = (new App.MarsDate(periodEndDate)).addDays(1).setFormat(App.MarsDate.MD_FORMAT_PW);
        //fieldValues['startMarsPeriod'] = nextPeriodMarsDate.toString();
        this.callParent([fieldValues]);
    },

    items: [{
        xtype: 'fsearchfield',
        name: 'clientFilter',
        fieldLabel: l10n.ns('tpm', 'BaseLine').value('ClientTreeDemandCode'),
        selectorWidget: 'clienttreesharesview',
        valueField: 'DemandCode',
        displayField: 'DemandCode',
        multiSelect: true,
        store: {
            type: 'directorystore',
            model: 'App.model.tpm.clienttreesharesview.ClientTreeSharesView',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.clienttreesharesview.ClientTreeSharesView',
                    modelId: 'efselectionmodel'
                }, {
                    xclass: 'App.ExtTextFilterModel',
                    modelId: 'eftextmodel'
                }]
            },
            fixedFilters: {
                'demandfilter': {
                    property: 'DemandCode',
                    operation: 'NotEqual',
                    value: ''
                },
                'demandNullfilter': {
                    property: 'DemandCode',
                    operation: 'NotNull',
                    value: null
                }
            }
        }
    }, {
        xtype: 'datefield',
        name: 'startDate',
        fieldLabel: l10n.ns('tpm', 'BaseLine', 'importParam').value('startDate'),
        allowBlank: false,
        format: 'd.m.Y',
        vtype: 'dateRange',
        //validator: App.util.core.Util.maxMarsDateRangeValidator,
        //maxRange: 3,
        //maxRangeInterval: Ext.Date.YEAR,
        finishDateField: 'endDate'
    }, {
        xtype: 'datefield',
        name: 'endDate',
        fieldLabel: l10n.ns('tpm', 'BaseLine', 'importParam').value('endDate'),
        allowBlank: false,
        format: 'd.m.Y',
        vtype: 'dateRange',
        //validator: App.util.core.Util.maxMarsDateRangeValidator,
        //maxRange: 3,
        //maxRangeInterval: Ext.Date.YEAR,
        startDateField: 'startDate',
        // переопределение нужно для того чтобы при преобразовании значения в дату подклеивать время 23:59:59 для корректной фильтрации
        parseDate: function (value) {
            if (!value || Ext.isDate(value)) {
                return value;
            }

            var me = this,
                val = me.safeParse(value, me.format),
                altFormats = me.altFormats,
                altFormatsArray = me.altFormatsArray,
                i = 0,
                len;

            if (!val && altFormats) {
                altFormatsArray = altFormatsArray || altFormats.split('|');
                len = altFormatsArray.length;
                for (; i < len && !val; ++i) {
                    val = me.safeParse(value, altFormatsArray[i]);
                }
            }
            return new Date(val.setHours(23, 59, 59));
        },
    }, {
        xtype: 'booleancombobox',
        name: 'clearTable',
        fieldLabel: l10n.ns('tpm', 'BaseLine', 'importParam').value('clearTable'),
        value: true,
        store: {
            type: 'booleannonemptystore',
        },
        isConstrain: false
    }]
});