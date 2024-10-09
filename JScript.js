function IsUserNameContained(username, password) {
    if (username == '')
        return false;
    if (password.toUpperCase().indexOf(username.toUpperCase()) > -1) {
        return true;
    } else {
        return false;
    }
}

function IsValidPolicy(pwd, val) {
    var exprPolicy = val.getAttribute('validationexpression')

    var exprMaxAllowedRepetitions = '';
    var exprRest = '';

    try {
        exprMaxAllowedRepetitions = exprPolicy.match(/(^.*?\\2.*?\$\))/)[0];

        exprMaxAllowedRepetitions = exprMaxAllowedRepetitions + '.*$';
    } catch (ex) {
    }

    try {
        exprRest = exprPolicy.match(/^(.*?\\2.*?\$\))?(.*)$/)[2];
    }
    catch (ex) {
    }

    if (pwd.match(exprRest) == null) {
        return false;
    }

    if (exprMaxAllowedRepetitions != '') {
        return (pwd.match(exprMaxAllowedRepetitions) != null);
    }

    return true;
}

function PasswordPolicyValidatorValidate(ClientID, UserNameControlClientID) {
    var val = document.getElementById(ClientID);

    var pwd = ValidatorGetValue(val.getAttribute('controltovalidate'));

    if (!IsValidPolicy(pwd, val)) {
        return false;
    }

    if (UserNameControlClientID != '') {
        var username = ValidatorGetValue(UserNameControlClientID);
        var pwd = ValidatorGetValue(val.getAttribute('controltovalidate'));

        if (IsUserNameContained(username, pwd)) {
            return false;
        }
    }
    return true;
}

function GetMaxAllowedRepetitionsExpression(array2, categoryPos1) {
    var expr1 = '^(?=^((.)(?!(.*?\\2){$$MaxNoOfAllowedRepetitions$$,$$MaxNoOfAllowedRepetitions$$}))+$).*$';

    for (i = 0; i < array2.length; i++) {
        if ((array2[i][1]).indexOf('Max') >= 0) {
            expr1 = expr1.replace('$$' + array2[i][1] + '$$', array2[i][categoryPos1 + 1] != null ? array2[i][categoryPos1 + 1] : '');
            expr1 = expr1.replace('$$(?!Max)' + array2[i][1] + '$$', array2[i][categoryPos1] != null ? array2[i][categoryPos1] : '');
        }
    }

    expr1 = expr1.replace(/[$]{2,2}Max.*?[$]{2,2}/gi, '');

    return expr1;
}

function IsMaxAllowedRepetitionsSpecified(array1) {
    for (i = 0; i < array1.length; i++) {
        if ((array1[i][1]).indexOf('Max') < 0) {
            return true
        }
    }
    return false;
}

function GetExpression(arrayStr, categoryPos, Unicase, lowercase) {
    Unicase = (Unicase == '' || Unicase == null ? 'A-Z' : Unicase);
    lowercase = (lowercase == '' || lowercase == null ? 'a-z' : lowercase);
    
    var expr = '^(?=.{$$MinPasswordLength$$,$$MinPasswordLength$$})(?=([^0-9]*?\d){$$MinNoOfNumbers$$,$$MinNoOfNumbers$$})(?=([^' + Unicase + ']*?[' + Unicase + ']){$$MinNoOfUniCaseChars$$,$$MinNoOfUpperUniChars$$})(?=([^' + lowercase + ']*?[' + lowercase + ']){$$MinNoOfLowerCaseChars$$,$$MinNoOfLowerCaseChars$$})(?=([^' + Unicase + ']*?[' + Unicase + ']){$$MinNoOfUpperCaseChars$$,$$MinNoOfUpperCaseChars$$})(?=([' + Unicase + lowercase + '0-9]*?[^' + Unicase + lowercase + '0-9]){$$MinNoOfSymbols$$,$$MinNoOfSymbols$$}).*$';

    for (i = 0; i < arrayStr.length; i++) {
        if ((arrayStr[i][1]).indexOf('Max') < 0) {
            expr = expr.replace('$$' + arrayStr[i][1] + '$$', arrayStr[i][categoryPos + 1]); expr = expr.replace('$$' + arrayStr[i][1] + '$$', arrayStr[i][categoryPos + 2] != null ? arrayStr[i][categoryPos + 2] - 1 : '');
        }
        else
            expr = expr.replace('$$' + arrayStr[i][1] + '$$', arrayStr[i][categoryPos + 2] != null ? arrayStr[i][categoryPos + 2] : ''); expr = expr.replace('$$(?!Max)' + arrayStr[i][1] + '$$', arrayStr[i][categoryPos + 1] != null ? arrayStr[i][categoryPos + 1] : '');
    }
    while (expr.match(/[$]{2,2}.*?[$]{2,2}/)) {
        expr = expr.replace(/[$]{2,2}.*?[$]{2,2}/, '0'); expr = expr.replace(/[$]{2,2}.*?[$]{2,2}/, '');
    }
   
    return expr;
}

function IsLowerCategory(category1, category2, arrCategories) {
    if (category1 == '' || category2 == '') {
        return arrCategories[0];
    }
    if (arrCategories.length > 0) {
        var IsCategory2Matched = false;
        for (m = 0; m < arrCategories.length; m++) {
            if (arrCategories[m] == category1) {
                return category1;
            }
            if (arrCategories[m] == category2) {
                return category2;
            }
        }
    }
}

function GetStrength(arr, arrCategories, ClientID, arrUnicode) {
    var val = document.getElementById(ClientID);
    var pwd = ValidatorGetValue(val.getAttribute('controltovalidate'));

    var lastMatchCategory = 'Strength did not match.';

    if (arrCategories.length > 0) {
        var lastMatchCategory = arrCategories[0];

        for (k = 0; k < arrCategories.length; k++) {
            if (pwd.match(GetExpression(arr, k + 1, arrUnicode[0], arrUnicode[1] != null ? arrUnicode[1] : '')) != null) {
                lastMatchCategory = arrCategories[k];
            }
            else
                break;
        }

        var lastMatchCategoryAllowedRepetitions = '';
        if (IsMaxAllowedRepetitionsSpecified(arr)) {
            for (k = 0; k < arrCategories.length; k++) {
                if (pwd.match(GetMaxAllowedRepetitionsExpression(arr, k + 1)) != null) {
                    lastMatchCategoryAllowedRepetitions = arrCategories[k];
                }
            }
        }
    }

    return IsLowerCategory(lastMatchCategory, lastMatchCategoryAllowedRepetitions, arrCategories);
}

function ShowStrengthColour(strength, arrCategories, arrColour, labelID) {
    for (n = 0; n < arrCategories.length; n++) {
        if (arrCategories[n] == strength) {
            break;
        }
    }

    var lblMessage = document.getElementById(labelID);

    lblMessage.innerText = strength;
    lblMessage.style.color = arrColour[n];
}