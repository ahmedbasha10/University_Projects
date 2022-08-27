# what I did:
# -----------Data_Cleaning: *missing_values *num_to_ctg
# next_Time/////-------------->>>>> OUTLIERS
import pandas as pd
import numpy as np
from sklearn import metrics
from sklearn.linear_model import LogisticRegression
from sklearn.metrics import accuracy_score
from sklearn.metrics import classification_report, confusion_matrix
from sklearn.model_selection import train_test_split

# we have diffrent types of Features, specificly 3 typrs here
# 1. categorical_Features 2. numerical_Features 3. Ordinal_Features

####################################3in this section we are cleaning our data#########################################

Loan_data = pd.read_csv("train_u6lujuX_CVtuZ9i.csv")

##test_set = pd.read_csv("test_Y3wMUE5_7gLdaTN.csv")

Loan_data.Gender = Loan_data.Gender.map({'Male': 1, 'Female': 0})
Loan_data.Married = Loan_data.Married.map({'Yes': 1, 'No': 0})
Loan_data.Education = Loan_data.Education.map({'Graduate': 1, 'Not Graduate': 0})
Loan_data.Self_Employed = Loan_data.Self_Employed.map({'Yes': 1, 'No': 0})
Loan_data.Property_Area = Loan_data.Property_Area.map({'Urban': 0, 'Rural': 1, 'Semiurban': 2})
Loan_data.Loan_Status = Loan_data.Loan_Status.map({'Y': 1, 'N': 0})

Loan_data = Loan_data.drop(['Loan_Amount_Term', 'Loan_ID'], axis=1)  # what is axis?, what does it do?

# 1. we Convert Categorical to Numeric variables

# we kept in mind "dependents" because through the data set we have only persons with 3+ dependents or not!!!!

Loan_data['Dependents'].replace('3+', 3, inplace=True)

# Loan_data['Loan_Status'].replace('N',0, inplace=True)
# Loan_data['Loan_Status'].replace('Y',1,inplace=True)


# 2. treatnig missing values/ missing values imputation

# categorical_data we filled the missing values with mode
# numerical_data we filed the missing values with median


Loan_data['Gender'].fillna(Loan_data['Gender'].mode()[0], inplace=True)
Loan_data['Married'].fillna(Loan_data['Married'].mode()[0], inplace=True)
Loan_data['Dependents'].fillna(Loan_data['Dependents'].mode()[0], inplace=True)
Loan_data['Self_Employed'].fillna(Loan_data['Self_Employed'].mode()[0], inplace=True)
Loan_data['Credit_History'].fillna(Loan_data['Credit_History'].mode()[0], inplace=True)
Loan_data['CoapplicantIncome'].dropna(inplace=True)

# Loan_data['Loan_Amount_Term'].fillna(Loan_data['Loan_Amount_Term'].mode()[0], inplace=True)  #we have a little problem!!!!!!!!!!!!!1

# numerical_data -> median

Loan_data['LoanAmount'].fillna(Loan_data['LoanAmount'].median(), inplace=True)

output = Loan_data['Loan_Status']
Loan_data.drop('Loan_Status', axis=1, inplace=True)

model = LogisticRegression(solver='liblinear', random_state=0)
x_train, x_test, y_train, y_test = train_test_split(Loan_data, output, test_size=0.3, random_state=0)
model.fit(x_train, y_train)
prediction = model.predict(x_test)
print(metrics.accuracy_score(prediction, y_test))