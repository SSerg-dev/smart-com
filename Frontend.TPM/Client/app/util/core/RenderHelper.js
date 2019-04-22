Ext.define('App.util.core.RenderHelper', {
    alternateClassName: 'App.RenderHelper',
    singleton: true,

    getLocalizedRenderer: function (entityName, enumTypeName) {
        return function (value) {
            if (!Ext.isEmpty(value)) {
                //TODO: add module's name in ns
                return l10n.ns(entityName, enumTypeName).value(value);
            }
        }
    },

    getBooleanRenderer: function (trueText, falseText) {
        return function (value) {
            if (Ext.isEmpty(value)) {
                return '&#160;'; //пробел
            } else if (!value || value === 'false') {
                return falseText || l10n.ns('core', 'booleanValues').value('false')
            } else {
                return trueText || l10n.ns('core', 'booleanValues').value('true');
            }
        }
    },

    getRealValue: function (value, row) {
        return (value == null) ? null : value;
    },

    getShareTemplateRenderer: function () {

        var getChar = function (products, sourceChar) {
            if (products) {
                var p = Ext.Array.findBy(products, function (item) {
                    return item.get('OrdNumber') == sourceChar;
                });
                return p === null ? '-' : p.get('OrdNumber');
            } else {
                return sourceChar;
            }
        }

        return function (value, metaData, record, rowIndex, colIndex, store, view) {
            // Получить список продуктов для тестирования
            var products = view.up('productsharetemplate').productList;
            var val = '';
            var str = (value || '').toString();
            for (var i = 0; i < str.length; i++) {
                // Проверить наличие номера в списке тестируемых продуктов. Если такого нет - прочерк
                var ch = getChar(products, str[i]);
                var item = Ext.util.Format.htmlEncode(ch);
                val += Ext.String.format('<span class="letter">{0}</span>', item);
            }
            return Ext.String.format('<div class="share-template">{0}</div>', val);
        }
    },

    getAge: function (value) {
        if (value != null) {
            var monthsCount = parseInt(value);
            if (monthsCount > 0) {
                var yearsCount = Math.floor(monthsCount / 12);
                var monthsLeft = Math.floor(monthsCount % 12);

                if (monthsCount < 0) {
                    return '';
                }
                else {
                    if (yearsCount == 0) {
                        if (monthsLeft > 0) {
                            return monthsLeft.toString() + ' мес.';
                        }
                        else {
                            return 'менee 1 мес.';
                        }
                    }
                    else if (yearsCount == 1) {
                        if (monthsLeft > 0) {
                            return yearsCount.toString() + ' год ' + monthsLeft.toString() + ' мес.';
                        }
                        else {
                            return yearsCount.toString() + ' год';
                        }
                    }
                    else if (yearsCount > 1 && yearsCount < 5) {
                        if (monthsLeft > 0) {
                            return yearsCount.toString() + ' годa ' + monthsLeft.toString() + ' мес.';
                        }
                        else {
                            return yearsCount.toString() + ' годa';
                        }
                    }
                    else if (yearsCount > 1 && yearsCount >= 5) {
                        if (monthsLeft > 0) {
                            return yearsCount.toString() + ' лет ' + monthsLeft.toString() + ' мес.';
                        }
                        else {
                            return yearsCount.toString() + ' лет';
                        }
                    }
                }
            }
            else {
                return '';
            }
        }
        else {
            return '';
        }
    }

});