Ext.define('App.model.tpm.baseclienttreeview.BaseClientTreeView', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'BaseClientTreeView',
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
        // ЧТО ЭТО? Угадай!
        { name: 'BOIstring', hidden: true, type: 'string', mapping: 'BOI'}
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'BaseClientTreeViews',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    },
});