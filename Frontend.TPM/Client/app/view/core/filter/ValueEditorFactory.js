Ext.define('App.view.core.filter.ValueEditorFactory', {
    singleton: true,

    typeXTypeMap: {
        'string': 'textfield',
        'int': 'numberfield',
        'float': 'numberfield',
        'boolean': 'booleancombobox', //'tri-checkbox',//'checkbox', 'booleancombobox',
        'date': 'datefield'
    },

    newTypeConfigMap: {
        'numberfield': {
            'minValue': 'null',
            'allowDecimals': 'true',
            'decimalSeparator': ','
        }
    },

    constructor: function () {
        this.callParent(arguments);

        this.valueTypeFactoryMap = {
            'field': this.createFieldValueCmp,
            'range': this.createRangeValueCmp,
            'list': this.createListValueCmp,
            'searchlist': this.createSearchListValueCmp,
            'atom': this.createAtomValueCmp
        };
    },

    createEditor: function (entry, currentEditor) {
        var valueType = entry.getValueType(),
            fieldType = entry.getCorrectedFieldType(entry.get('fieldType')),
            factory = this.valueTypeFactoryMap[valueType] || this.createNullValueCmp,
            currentEditorSign = currentEditor && currentEditor.getItemId(),
            editorSign = valueType + '-' + fieldType;

        var atomConfig = entry.editors[fieldType] || {},
            valueConfig = entry.values[fieldType] || {},
            additionalTypeConfig = this.newTypeConfigMap[this.typeXTypeMap[fieldType]] || {};

        var modelKeyFields = entry.model.getFields().filter(function (field) { return field['isKey'] === true; });

        if (!modelKeyFields.some(function (field) { return field['name'] == entry.data['id']; })) {
            for (var propertyName in additionalTypeConfig) {
                var propertyValue = additionalTypeConfig[propertyName];
                atomConfig[propertyName] = propertyValue;
            }
        } else {
            atomConfig['minValue'] = 0;
            atomConfig['allowDecimals'] = false;
        } 
        if (fieldType === 'int') {
            atomConfig['allowDecimals'] = false;
        }
        if (Ext.isString(atomConfig)) {
            atomConfig = { xtype: atomConfig };
        }

        atomConfig = Ext.applyIf(Ext.clone(atomConfig), { xtype: this.typeXTypeMap[fieldType] });

        return currentEditorSign === editorSign
            ? currentEditor
            : factory.apply(this, [entry, atomConfig, valueConfig, editorSign]);
    },

    createAtomValueCmp: function (entry, cfg, valueCfg, sign) {
        return Ext.widget(Ext.apply(cfg, {
            itemId: sign
        }));
    },

    createRangeValueCmp: function (entry, cfg, valueCfg, sign) {
        var vCfg = valueCfg[entry.getValueType()] || {};

        return Ext.widget('rangevalueeditor', Ext.applyIf({
            itemId: sign,
            atomConfig: cfg
        }, vCfg));
    },

    createListValueCmp: function (entry, cfg, valueCfg, sign) {
        var vCfg = valueCfg[entry.getValueType()] || {};

        return Ext.widget('multiselectfield', Ext.applyIf({
            itemId: sign,
            atomConfig: cfg,
            allowRange: entry.isAllowedRange()
        }, vCfg));
    },

    createSearchListValueCmp: function (entry, cfg, valueCfg, sign) {
        //cfg.xtype - fsearchfield или treefsearchfield
        return Ext.widget(cfg.xtype, Ext.applyIf({
            itemId: sign,
            multiSelect: entry.get('operation') === 'In'
        }, cfg));
    },

    createFieldValueCmp: function (entry, cfg, valueCfg, sign) {
        var model = entry.getModel(),
            moduleName = App.Util.getSubdirectory(Ext.getClassName(model)),
            data = entry.getAllowedModelFields().map(function (field) {
                return {
                    id: field.mapping || field.name,
                    text: l10n.ns(moduleName, model.prototype.breezeEntityType).value(field.name)
                };
            }, this),
            cmp = Ext.widget('fieldvalueeditor', {
                itemId: sign
            });

        cmp.getStore().loadData(data);

        return cmp;
    },

    createNullValueCmp: function () {
        return null;
    }

});