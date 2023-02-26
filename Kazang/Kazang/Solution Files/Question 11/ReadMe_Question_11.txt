*********************************************
*************QUESTION 11*********************
*********************************************

Is this the best approach to take when to control "Master" record data? If no, explain the best approach. If yes, explain other possible approaches

There are a number of things to consider when taking control of "Master" record data in order to optimize it:

1. there should also be no more than one "authorized" subEntity on a masterEntity record.  Or the lastest will overwrite and take precedence, to sync up to Master.
2. Duplicate Detection of SubEntities, and/or properties.
3. Limitation if record has many properties to sync. 2 mins limitation on plugins.
4. Might not cater for situation when previously authorized, and then becomes unauthorized.
5. A unique number should be created for properties via AutoNumber solution ideally.  Using name by default, can lead to multiple properties with same name as duplicates.  And wont sync correctly.  Will take first one for example.
6. Status Reason not on masterEntity form.
7. A super user may be needed to override changes. In the event the form becomes locked, and users made a mistake.  This can lead to increased service requests/calls to unlock or use "God Mode".

