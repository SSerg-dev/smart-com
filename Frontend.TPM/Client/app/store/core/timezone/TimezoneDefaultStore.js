// ----------------------------------------------------------------------
// <copyright file="TimezoneDefaultStore.js" company="Смарт-Ком">
//     Copyright statement. All right reserved
// </copyright>
// Проект Transportation Workflow System
// Версия 2.0
// Автор: Максим Молоканов (EMail: maxim.molokanov@smart-com.su)
// ----------------------------------------------------------------------

Ext.require('App.model.core.timezone.TimezoneModel');
Ext.define('App.store.core.timezone.TimezoneDefaultStore', {
    extend: 'Ext.data.Store',
    autoLoad: true,
    model: 'App.model.core.timezone.TimezoneModel',
    proxy: {
        type: 'memory',
        reader: {
            type: 'json',
            root: 'data'
        }
    },
    data: {
        data: [
            { Id : 'Default', Name : 'По умолчанию' },
            { Id : 'Dateline Standard Time', Name : '(GMT-12:00) Меридиан смены дат (запад)' },
            { Id : 'Samoa Standard Time', Name : '(GMT-11:00) о. Мидуэй, Самоа' },
            { Id : 'Hawaiian Standard Time', Name : '(GMT-10:00) Гавайи' },
            { Id : 'Alaskan Standard Time', Name : '(GMT-09:00) Аляска' },
            { Id : 'Pacific Standard Time', Name : '(GMT-08:00) Тихоокеанское время (США и Канада); Тихуана' },
            { Id : 'US Mountain Standard Time', Name : '(GMT-07:00) Аризона' },
            { Id : 'Mountain Standard Time', Name : '(GMT-07:00) Горное время (США и Канада)' },
            { Id : 'Mexico Standard Time 2', Name : '(GMT-07:00) Ла Пас, Мазатлан, Чихуахуа' },
            { Id : 'Mexico Standard Time', Name : '(GMT-06:00) Гвадалахара, Мехико, Монтеррей' },
            { Id : 'Canada Central Standard Time', Name : '(GMT-06:00) Саскачеван' },
            { Id : 'Central America Standard Time', Name : '(GMT-06:00) Центральная Америка' },
            { Id : 'Central Standard Time', Name : '(GMT-06:00) Центральное время (США и Канада)' },
            { Id : 'SA Pacific Standard Time', Name : '(GMT-05:00) Богота, Лима, Кито' },
            { Id : 'Eastern Standard Time', Name : '(GMT-05:00) Восточное время (США и Канада)' },
            { Id : 'US Eastern Standard Time', Name : '(GMT-05:00) Индиана (восток)' },
            { Id : 'Atlantic Standard Time', Name : '(GMT-04:00) Атлантическое время (Канада)' },
            { Id : 'SA Western Standard Time', Name : '(GMT-04:00) Каракас, Ла Пас' },
            { Id : 'Pacific SA Standard Time', Name : '(GMT-04:00) Сантьяго' },
            { Id : 'Newfoundland Standard Time', Name : '(GMT-03:30) Ньюфаундленд' },
            { Id : 'E. South America Standard Time', Name : '(GMT-03:00) Бразилия' },
            { Id : 'SA Eastern Standard Time', Name : '(GMT-03:00) Буэнос-Айрес, Джорджтаун' },
            { Id : 'Greenland Standard Time', Name : '(GMT-03:00) Гренландия' },
            { Id : 'Mid-Atlantic Standard Time', Name : '(GMT-02:00) Среднеатлантическое время' },
            { Id : 'Azores Standard Time', Name : '(GMT-01:00) Азорские о-ва' },
            { Id : 'Cape Verde Standard Time', Name : '(GMT-01:00) о-ва Зеленого мыса' },
            { Id : 'GMT Standard Time', Name : '(GMT) Время по Гринвичу: Дублин, Лондон, Лиссабон, Эдинбург' },
            { Id : 'Greenwich Standard Time', Name : '(GMT) Касабланка, Монровия' },
            { Id : 'W. Europe Standard Time', Name : '(GMT+01:00) Амстердам, Берлин, Берн, Вена, Рим, Стокгольм' },
            { Id : 'Central Europe Standard Time', Name : '(GMT+01:00) Белград, Братислава, Будапешт, Любляна, Прага' },
            { Id : 'Romance Standard Time', Name : '(GMT+01:00) Брюссель, Копенгаген, Мадрид, Париж' },
            { Id : 'Central European Standard Time', Name : '(GMT+01:00) Варшава, Загреб, Сараево, Скопье' },
            { Id : 'W. Central Africa Standard Time', Name : '(GMT+01:00) Западная Центральная Африка' },
            { Id : 'GTB Standard Time', Name : '(GMT+02:00) Афины, Бейрут, Киев, Минск, Стамбул' },
            { Id : 'E. Europe Standard Time', Name : '(GMT+02:00) Бухарест' },
            { Id : 'FLE Standard Time', Name : '(GMT+02:00) Вильнюс, Киев, Рига, София, Таллинн, Хельсинки' },
            { Id : 'Israel Standard Time', Name : '(GMT+02:00) Иерусалим' },
            { Id : 'Egypt Standard Time', Name : '(GMT+02:00) Каир' },
            { Id : 'South Africa Standard Time', Name : '(GMT+02:00) Хараре, Претория' },
            { Id : 'Arabic Standard Time', Name : '(GMT+03:00) Багдад' },
            { Id : 'Arab Standard Time', Name : '(GMT+03:00) Кувейт, Эр-Рияд' },
            { Id : 'E. Africa Standard Time', Name : '(GMT+03:00) Найроби' },
            { Id : 'Iran Standard Time', Name : '(GMT+03:30) Тегеран' },
            { Id : 'Arabian Standard Time', Name : '(GMT+04:00) Абу-Даби, Мускат' },
            { Id : 'Caucasus Standard Time', Name : '(GMT+04:00) Баку, Ереван, Тбилиси' },
            { Id : 'Russian Standard Time', Name : '(GMT+04:00) Волгоград, Москва, Санкт-Петербург' },
            { Id : 'Afghanistan Standard Time', Name : '(GMT+04:30) Кабул' },
            { Id : 'Ekaterinburg Standard Time', Name : '(GMT+05:00) Екатеринбург' },
            { Id : 'West Asia Standard Time', Name : '(GMT+05:00) Исламабад, Карачи, Ташкент' },
            { Id : 'India Standard Time', Name : '(GMT+05:30) Бомбей, Калькутта, Мадрас, Нью-Дели' },
            { Id : 'Nepal Standard Time', Name : '(GMT+05:45) Катманду' },
            { Id : 'Central Asia Standard Time', Name : '(GMT+06:00) Астана, Дхака' },
            { Id : 'N. Central Asia Standard Time', Name : '(GMT+06:00) Омск, Новосибирск, Алма-Ата' },
            { Id : 'Sri Lanka Standard Time', Name : '(GMT+06:00) Шри Джаяварденепура' },
            { Id : 'Myanmar Standard Time', Name : '(GMT+06:30) Рангун' },
            { Id : 'SE Asia Standard Time', Name : '(GMT+07:00) Бангкок, Джакарта, Ханой' },
            { Id : 'North Asia Standard Time', Name : '(GMT+07:00) Красноярск' },
            { Id : 'China Standard Time', Name : '(GMT+08:00) Гонконг, Пекин, Урумчи' },
            { Id : 'North Asia East Standard Time', Name : '(GMT+08:00) Иркутск, Улан-Батор' },
            { Id : 'Singapore Standard Time', Name : '(GMT+08:00) Куала-Лумпур, Сингапур' },
            { Id : 'W. Australia Standard Time', Name : '(GMT+08:00) Перт' },
            { Id : 'Taipei Standard Time', Name : '(GMT+08:00) Тайпей' },
            { Id : 'Tokyo Standard Time', Name : '(GMT+09:00) Осака, Саппоро, Токио' },
            { Id : 'Korea Standard Time', Name : '(GMT+09:00) Сеул' },
            { Id : 'Yakutsk Standard Time', Name : '(GMT+09:00) Якутск' },
            { Id : 'Cen. Australia Standard Time', Name : '(GMT+09:30) Аделаида' },
            { Id : 'AUS Central Standard Time', Name : '(GMT+09:30) Дарвин' },
            { Id : 'E. Australia Standard Time', Name : '(GMT+10:00) Брисбейн' },
            { Id : 'Vladivostok Standard Time', Name : '(GMT+10:00) Владивосток' },
            { Id : 'West Pacific Standard Time', Name : '(GMT+10:00) Гуам, Порт Моресби' },
            { Id : 'AUS Eastern Standard Time', Name : '(GMT+10:00) Канберра, Мельбурн, Сидней' },
            { Id : 'Tasmania Standard Time', Name : '(GMT+10:00) Хобарт' },
            { Id : 'Central Pacific Standard Time', Name : '(GMT+11:00) Магадан, Сахалин, Соломоновы о-ва' },
            { Id : 'Fiji Standard Time', Name : '(GMT+12:00) Камчатка, Фиджи, Маршалловы о-ва' },
            { Id : 'New Zealand Standard Time', Name : '(GMT+12:00) Окленд, Веллингтон' },
            { Id : 'Tonga Standard Time', Name : '(GMT+13:00) Нуку-алофа' }
        ]
    },

    constructor: function (config) {
        this.callParent(arguments);
        Ext.apply(this, config);
    }
});
