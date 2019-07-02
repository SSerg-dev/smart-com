Ext.define('App.model.tpm.clienttreesharesview.ClientTreeSharesView', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'ClientTreeSharesView',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'BOI', type: 'int', hidden: false, isDefault: true },
        { name: 'ResultNameStr', type: 'string', hidden: false, isDefault: true },
        {
            name: 'ShortName', type: 'string', hidden: true, convert: function (v, record) {
                var shortName = v;

                if (shortName.length == 0) {
                    var name = record.get('ResultNameStr');
                    var index = name.lastIndexOf('>') + 1;

                    shortName = name.substr(index).trim();
                    record.data.ShortName = shortName;
                }

                return shortName;
            }
        },
        { name: 'LeafShare', type: 'int', hidden: false, isDefault: true },
        { name: 'DemandCode', type: 'string', hidden: false, isDefault: true },

    ],
    proxy: {
        type: 'breeze',
        resourceName: 'ClientTreeSharesViews',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    },
});