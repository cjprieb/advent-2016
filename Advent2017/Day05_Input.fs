﻿module Day05_Input
    let instructions = [|
        1
        2
        -1
        -2
        1
        -3
        -1
        1
        0
        -4
        -8
        -7
        -2
        0
        -2
        -11
        1
        -2
        0
        0
        -11
        -17
        -18
        -1
        -12
        -21
        -15
        -24
        -8
        -5
        0
        -17
        -8
        -5
        -24
        -16
        -16
        -21
        -5
        -7
        -13
        -11
        -2
        -27
        -29
        -38
        -2
        2
        -27
        -10
        -9
        -32
        -3
        -1
        -6
        -50
        -21
        -47
        -47
        -16
        -48
        -19
        -53
        -25
        -57
        -42
        -64
        -21
        -59
        -3
        -51
        -66
        -44
        -42
        -45
        -6
        -18
        -28
        -18
        -48
        -21
        -15
        -4
        -10
        -49
        -72
        -56
        -47
        -41
        -74
        -38
        -60
        -28
        -10
        -32
        -1
        -9
        -40
        -10
        -6
        -58
        -92
        -8
        -94
        -99
        -93
        -33
        -31
        -84
        -28
        -39
        -105
        -23
        -76
        -35
        -71
        -100
        -102
        -29
        -86
        -70
        -30
        -8
        0
        -109
        1
        -22
        -24
        -92
        -21
        -103
        -127
        -67
        0
        -68
        -31
        -71
        -111
        -26
        -123
        -39
        -116
        -15
        -86
        -85
        -137
        -127
        -134
        -145
        -29
        -123
        -19
        -43
        -152
        -122
        -148
        -129
        -97
        -39
        -28
        -49
        -93
        -110
        -103
        -130
        1
        -114
        -146
        -99
        -128
        -118
        -32
        -48
        -115
        -155
        -26
        -37
        -65
        -48
        -71
        -6
        -137
        -178
        -111
        -139
        -127
        -160
        -172
        -98
        -38
        -156
        -11
        -62
        -187
        -53
        2
        -117
        -3
        -31
        -143
        -41
        -47
        -169
        -162
        -158
        -12
        -69
        -114
        -180
        -155
        -125
        -64
        -176
        -184
        -202
        -116
        -74
        -98
        -205
        -84
        -152
        -54
        -102
        -165
        -138
        -140
        -180
        -96
        -98
        -109
        -81
        -199
        -137
        -56
        -74
        -179
        -175
        -114
        -124
        -15
        -234
        -219
        -51
        -41
        -144
        -134
        -161
        -59
        -128
        -71
        -22
        -165
        -222
        -70
        -65
        -51
        -43
        -86
        -198
        -238
        -119
        -31
        -195
        -87
        -102
        -30
        -73
        -76
        -153
        -238
        -8
        -73
        -63
        -148
        -42
        -16
        -228
        -243
        -235
        -160
        -107
        -235
        -29
        -188
        -202
        -42
        -215
        -159
        -134
        -172
        -263
        -188
        -124
        -34
        -206
        -15
        -138
        -184
        -20
        -32
        -271
        -103
        -203
        -129
        -177
        -69
        -107
        -265
        -68
        -299
        -161
        -148
        -182
        0
        -207
        -106
        -68
        -92
        -53
        -52
        -288
        -3
        -211
        -143
        -204
        -126
        -152
        -106
        -232
        -153
        -234
        -62
        -124
        -131
        -42
        -297
        -332
        -188
        -115
        -100
        -173
        -52
        -115
        -296
        -301
        -312
        -292
        -2
        -321
        -178
        -174
        -244
        -309
        -161
        -346
        -251
        -157
        -325
        -292
        -159
        -95
        0
        -124
        -69
        -324
        -223
        -89
        -359
        -242
        -239
        1
        -39
        -204
        -287
        -142
        -123
        -363
        -218
        -197
        -136
        -20
        -304
        -281
        -83
        -7
        -129
        -315
        -76
        -349
        -141
        -318
        -369
        -346
        -161
        -141
        -110
        -279
        -5
        -86
        -348
        -59
        -255
        -266
        -355
        -110
        -14
        -339
        -109
        -44
        -38
        -10
        -164
        -214
        -265
        -412
        -72
        -413
        -271
        -343
        -124
        -352
        -304
        -124
        -381
        -258
        -8
        -235
        -288
        -27
        -296
        -179
        -392
        -336
        -255
        -114
        -15
        -407
        -296
        -29
        -352
        -419
        -190
        -308
        -2
        -430
        -157
        -379
        -220
        -179
        -77
        -337
        -61
        -48
        -64
        -197
        -408
        -284
        -84
        -409
        -243
        -316
        -77
        -77
        -428
        -432
        -182
        -437
        -254
        -50
        -260
        -301
        -28
        -33
        -335
        -348
        -240
        -287
        -436
        -225
        -221
        -198
        -190
        -50
        -87
        -161
        -408
        0
        -14
        -225
        -105
        -188
        -290
        -349
        -57
        -45
        -20
        -384
        -36
        -264
        -359
        -52
        -21
        -328
        -194
        -432
        -113
        -475
        -391
        -86
        -407
        -18
        -435
        -206
        -317
        -254
        -369
        -373
        -127
        -405
        -309
        -154
        -480
        -271
        -71
        -306
        -381
        -252
        -253
        -420
        -40
        -349
        -403
        -44
        -256
        -33
        -429
        -10
        -461
        -405
        -216
        -329
        -201
        -498
        -392
        -149
        -419
        -85
        -408
        -248
        -88
        -322
        -438
        -381
        -100
        -445
        -412
        -215
        -220
        -83
        -436
        -411
        -555
        -372
        -232
        -309
        -151
        -214
        -219
        -268
        -123
        -90
        -241
        -508
        -134
        -74
        -296
        -505
        -240
        -161
        -477
        -63
        -118
        -293
        -69
        -197
        -88
        -520
        -170
        -37
        -114
        -234
        -36
        -225
        -116
        -36
        -195
        -363
        -75
        -137
        -7
        -506
        -124
        -556
        -15
        -327
        -74
        -367
        -505
        -29
        -296
        -281
        -180
        -420
        -119
        -449
        -502
        -204
        -294
        -484
        -515
        -74
        -337
        -256
        -479
        -471
        -27
        -614
        -354
        -369
        -607
        -244
        -578
        -195
        -215
        -407
        -552
        -247
        -514
        -434
        -291
        -521
        -99
        -598
        -292
        -400
        -594
        -381
        -602
        -260
        -79
        -441
        -444
        -146
        -451
        -502
        -215
        -81
        -577
        -652
        -507
        -264
        -588
        -431
        -401
        -103
        -282
        -125
        -259
        -615
        -321
        -271
        -84
        -84
        -323
        -650
        -79
        -289
        -522
        -129
        -343
        -441
        -186
        -561
        -244
        -186
        -296
        -272
        -258
        -308
        -390
        -677
        -367
        -186
        -604
        -104
        -481
        -394
        -31
        -663
        -493
        -608
        -142
        -86
        -356
        -581
        -131
        -11
        -92
        -258
        -552
        -176
        -244
        -208
        -564
        -9
        -558
        -256
        -439
        -460
        -641
        -457
        -715
        -328
        -291
        -172
        -380
        -406
        0
        -123
        -286
        -301
        -375
        -358
        -607
        -599
        -670
        -94
        -143
        -65
        -201
        -486
        -394
        -405
        -671
        -673
        -564
        -137
        -200
        -148
        -644
        -589
        -643
        -155
        -714
        -602
        -54
        -746
        -403
        -520
        -446
        -646
        -680
        -474
        -431
        -762
        -712
        -554
        -187
        -242
        -242
        -595
        -66
        -610
        -378
        -430
        -595
        -485
        -467
        -434
        -663
        -375
        -81
        -503
        -688
        -651
        -17
        -10
        -184
        -361
        -165
        -785
        -61
        -211
        -140
        -740
        -126
        -549
        -222
        -611
        -557
        -786
        -525
        -431
        -111
        -287
        -131
        -574
        -212
        -733
        -223
        -734
        -275
        -524
        -295
        -541
        -240
        -162
        -750
        -350
        -486
        -672
        -579
        -410
        -737
        -544
        -728
        -516
        -163
        -227
        -249
        -177
        -522
        -363
        -190
        -613
        -148
        -810
        -593
        -702
        -545
        -187
        -27
        -332
        -611
        -510
        -214
        -56
        -219
        -696
        -593
        -720
        -479
        -155
        -278
        -517
        -691
        -314
        -638
        -748
        -232
        -737
        -46
        -138
        -192
        -631
        -224
        -691
        -628
        -613
        -324
        -185
        -365
        -259
        -219
        -462
        -290
        -783
        -710
        -444
        -271
        -117
        -469
        -609
        -105
        -602
        -465
        -260
        -323
        -544
        -493
        -458
        -261
        -102
        -198
        -221
        -321
        -694
        -614
        -147
        -511
        -592
        -335
        -738
        -198
        -274
        -780
        -598
        -281
        -686
        -25
        -682
        -827
        -491
        -312
        -540
        -304
        -293
        2
        -238
        -614
        -22
        -380
        -194
        -167
        -167
        -569
        -170
        -184
        -104
        -327
        -401
        -654
        -926
        -571
        -181
        -809
        -552
        -767
        -579
        -823
        -620
        -660
        -853
        -448
        -720
        -872
        -898
        -45
        -154
        -409
        -399
        -950
        -393
        -782
        -376
        -65
        -644
        -654
        -523
        -24
        -767
        -419
        -183
        -143
        -98
        -792
        -485
        -923
        -360
        -173
        -879
        -847
        -732
        -962
        -643
        -392
        -117
        -4
        -932
        -253
        -298
        -381
        -339
        -796
        -274
        -79
        -586
        -567
        -425
        -541
        -329
        -800
        -878
        -519
        -111
        -224
        -304
        -560
        -183
        -604
        -952
        -229
        2
        -115
        -748
        -262
        -54
        -533
        -139
        -785
        -583
        -634
        -164
        -836
        -77
        -578
        -593
        -561
        -596
        -611
        -440
        -27
        -848
        -998
        -56
        -947
        -740
        -737
        -612
        -655
        -845
        -812
        -925
        -197
        -236
        -37
        -753
        -747
        -286
        -641
        -43
        -348
        -33
        -713
        -610
        -777
        -899
        -1005
        -264
        -193
        -928
        -193
        -412
        -213
        -228
        -1012
        -920
        -702
        -420
        -496
        -1019
        -386
        -645
        -804
        -795
        -12
        -810
        -117
        -454
        -266
        -1059
        -321
        -674
        -647
    |]
