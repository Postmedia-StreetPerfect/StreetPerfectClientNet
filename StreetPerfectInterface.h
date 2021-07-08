/*
***************************************************************************************************
Added for reference only
***************************************************************************************************
*/

#ifdef  WIN32

#ifdef  SPAA_CDECL_LIBRARY

#ifndef StreetPerfectExportDefinition
#define StreetPerfectExportDefinition long _cdecl
#endif

#ifndef StreetPerfectExportDeclaration
#define StreetPerfectExportDeclaration extern long _cdecl
#endif

#else

#ifndef StreetPerfectExportDefinition
#define StreetPerfectExportDefinition long _stdcall
#endif

#ifndef StreetPerfectExportDeclaration
#define StreetPerfectExportDeclaration extern long _stdcall
#endif

#endif

#else

#ifndef StreetPerfectExportDefinition
#define StreetPerfectExportDefinition long
#endif

#ifndef StreetPerfectExportDeclaration
#define StreetPerfectExportDeclaration extern long
#endif

#endif

/*
***************************************************************************************************
***************************************************************************************************
*/

#ifdef __cplusplus
extern "C" {
#endif

/*
***************************************************************************************************
***************************************************************************************************
*/

StreetPerfectExportDeclaration
StreetPerfectProcessAddress ( char * PS_ARG_IN_01,
                              char * PS_ARG_IN_02,
                              char * PS_ARG_IN_03,
                              char * PS_ARG_IN_04,
                              char * PS_ARG_IN_05,
                              char * PS_ARG_IN_06,
                              char * PS_ARG_IN_07,
                              char * PS_ARG_IN_08,
                              char * PS_ARG_OUT_09,
                              char * PS_ARG_OUT_10,
                              char * PS_ARG_OUT_11,
                              char * PS_ARG_OUT_12,
                              char * PS_ARG_OUT_13,
                              char * PS_ARG_OUT_14,
                              char * PS_ARG_OUT_15,
                              char * PS_ARG_OUT_16,
                              char * PS_ARG_OUT_17,
                              char * PS_ARG_OUT_18,
                              char * PS_ARG_OUT_19,
                              char * PS_ARG_OUT_20,
                              char * PS_ARG_OUT_21,
                              char * PS_ARG_OUT_22,
                              char * PS_ARG_OUT_23,
                              char * PS_ARG_OUT_24,
                              char * PS_ARG_OUT_25,
                              char * PS_ARG_OUT_26 );
/*
***************************************************************************************************
   NOTE:  Unused parameters must be passed in as null strings.
          Failure to do so may cause memory curruption and program failure.
***************************************************************************************************
   Redefinition of StreetPerfectProcessAddress function parameters for Canadian addresses
   PS_CAN_in_function = "CAN_AddressCorrection"
***************************************************************************************************
StreetPerfectProcessAddress ( PS_ARG_in_parameter_file,
                              PS_ARG_in_sub_function,
                              PS_CAN_in_recipient,
                              PS_CAN_in_reserved,
                              PS_CAN_in_address_line,
                              PS_CAN_in_city,
                              PS_CAN_in_province,
                              PS_CAN_in_postal_code,
                              PS_ARG_out_status_flag,
                              PS_ARG_out_status_messages,
                              PS_ARG_out_function_messages,
                              PS_CAN_out_recipient,
                              PS_CAN_out_reserved,
                              PS_CAN_out_address_line,
                              PS_CAN_out_city,
                              PS_CAN_out_province,
                              PS_CAN_out_postal_code,
                              PS_CAN_out_extra_information,
                              PS_CAN_out_unidentified_component,
                              PS_XXX_out_null_string_20,
                              PS_XXX_out_null_string_21,
                              PS_XXX_out_null_string_22,
                              PS_XXX_out_null_string_23,
                              PS_XXX_out_null_string_24,
                              PS_XXX_out_null_string_25,
                              PS_XXX_out_null_string_26 );
***************************************************************************************************
   Redefinition of StreetPerfectProcessAddress function parameters for Canadian addresses
   PS_CAN_in_function = "CAN_AddressParse"
***************************************************************************************************
StreetPerfectProcessAddress ( PS_ARG_in_parameter_file,
                              PS_ARG_in_sub_function,
                              PS_CAN_in_recipient,
                              PS_CAN_in_reserved,
                              PS_CAN_in_address_line,
                              PS_CAN_in_city,
                              PS_CAN_in_province,
                              PS_CAN_in_postal_code,
                              PS_ARG_out_status_flag,
                              PS_ARG_out_status_messages,
                              PS_ARG_out_function_messages,
                              PS_CAN_out_address_type,
                              PS_CAN_out_street_number,
                              PS_CAN_out_street_suffix,
                              PS_CAN_out_street_name,
                              PS_CAN_out_street_type,
                              PS_CAN_out_street_direction,
                              PS_CAN_out_unit_type,
                              PS_CAN_out_unit_number,
                              PS_CAN_out_service_type,
                              PS_CAN_out_service_number,
                              PS_CAN_out_service_area_name,
                              PS_CAN_out_service_area_type,
                              PS_CAN_out_service_area_qualifier,
                              PS_CAN_out_extra_information,
                              PS_CAN_out_unidentified_component );
***************************************************************************************************
   Redefinition of StreetPerfectProcessAddress function parameters for Canadian addresses
   PS_CAN_in_function = "CAN_AddressSearch"
***************************************************************************************************
StreetPerfectProcessAddress ( PS_ARG_in_parameter_file,
                              PS_ARG_in_sub_function,
                              PS_CAN_in_recipient,
                              PS_CAN_in_reserved,
                              PS_CAN_in_address_line,
                              PS_CAN_in_city,
                              PS_CAN_in_province,
                              PS_CAN_in_postal_code,
                              PS_ARG_out_status_flag,
                              PS_ARG_out_status_messages,
                              PS_ARG_out_function_messages,
                              PS_CAN_out_response_count,
                              PS_CAN_out_response_available,
                              PS_CAN_out_response_address_list,
                              PS_XXX_out_null_string_15,
                              PS_XXX_out_null_string_16,
                              PS_XXX_out_null_string_17,
                              PS_XXX_out_null_string_18,
                              PS_XXX_out_null_string_19,
                              PS_XXX_out_null_string_20,
                              PS_XXX_out_null_string_21,
                              PS_XXX_out_null_string_22,
                              PS_XXX_out_null_string_23,
                              PS_XXX_out_null_string_24,
                              PS_XXX_out_null_string_25,
                              PS_XXX_out_null_string_26 );
***************************************************************************************************
   Redefinition of StreetPerfectProcessAddress function parameters for American addresses
   PS_USA_in_function = "USA_AddressCorrection"
***************************************************************************************************
StreetPerfectProcessAddress ( PS_ARG_in_parameter_file,
                              PS_ARG_in_sub_function,
                              PS_USA_in_firm_name,
                              PS_USA_in_urbanization_name,
                              PS_USA_in_address_line,
                              PS_USA_in_city,
                              PS_USA_in_state,
                              PS_USA_in_zip_code,
                              PS_ARG_out_status_flag,
                              PS_ARG_out_status_messages,
                              PS_ARG_out_function_messages,
                              PS_USA_out_firm_name,
                              PS_USA_out_urbanization_name,
                              PS_USA_out_address_line,
                              PS_USA_out_city,
                              PS_USA_out_state,
                              PS_USA_out_zip_code,
                              PS_XXX_out_null_string_18,
                              PS_XXX_out_null_string_19,
                              PS_XXX_out_null_string_20,
                              PS_XXX_out_null_string_21,
                              PS_XXX_out_null_string_22,
                              PS_XXX_out_null_string_23,
                              PS_XXX_out_null_string_24,
                              PS_XXX_out_null_string_25,
                              PS_XXX_out_null_string_26 );
***************************************************************************************************
   Redefinition of StreetPerfectProcessAddress function parameters for American addresses
   PS_USA_in_function = "USA_AddressParse"
***************************************************************************************************
StreetPerfectProcessAddress ( PS_ARG_in_parameter_file,
                              PS_ARG_in_sub_function,
                              PS_USA_in_firm_name,
                              PS_USA_in_urbanization_name,
                              PS_USA_in_address_line,
                              PS_USA_in_city,
                              PS_USA_in_state,
                              PS_USA_in_zip_code,
                              PS_ARG_out_status_flag,
                              PS_ARG_out_status_messages,
                              PS_ARG_out_function_messages,
                              PS_USA_out_address_type,
                              PS_USA_out_street_number,
                              PS_USA_out_street_pre_direction,
                              PS_USA_out_street_name,
                              PS_USA_out_street_type,
                              PS_USA_out_street_post_direction,
                              PS_USA_out_secondary_type,
                              PS_USA_out_secondary_number,
                              PS_USA_out_service_type,
                              PS_USA_out_service_number,
                              PS_USA_out_delivery_point_barcode,
                              PS_USA_out_congressional_district,
                              PS_USA_out_county_name,
                              PS_USA_out_county_code,
                              PS_XXX_out_null_string_26 );
***************************************************************************************************
   Redefinition of StreetPerfectProcessAddress function parameters for American addresses
   PS_USA_in_function = USA_AddressSearch
***************************************************************************************************
StreetPerfectProcessAddress ( PS_ARG_in_parameter_file,
                              PS_ARG_in_sub_function,
                              PS_USA_in_firm_name,
                              PS_USA_in_urbanization_name,
                              PS_USA_in_address_line,
                              PS_USA_in_city,
                              PS_USA_in_state,
                              PS_USA_in_zip_code,
                              PS_ARG_out_status_flag,
                              PS_ARG_out_status_messages,
                              PS_ARG_out_function_messages,
                              PS_USA_out_response_count,
                              PS_USA_out_response_available,
                              PS_USA_out_response_address_list,
                              PS_XXX_out_null_string_15,
                              PS_XXX_out_null_string_16,
                              PS_XXX_out_null_string_17,
                              PS_XXX_out_null_string_18,
                              PS_XXX_out_null_string_19,
                              PS_XXX_out_null_string_20,
                              PS_XXX_out_null_string_21,
                              PS_XXX_out_null_string_22,
                              PS_XXX_out_null_string_23,
                              PS_XXX_out_null_string_24,
                              PS_XXX_out_null_string_25,
                              PS_XXX_out_null_string_26 );
***************************************************************************************************
*/

/*
***************************************************************************************************
***************************************************************************************************
*/
StreetPerfectExportDeclaration
StreetPerfectBatchProcess ( char * PS_ARG_in_parameter_file,
                            char * PS_ARG_out_status_flag,
                            char * PS_ARG_out_status_messages );
/*
***************************************************************************************************
***************************************************************************************************
*/
StreetPerfectExportDeclaration
StreetPerfectConnect ( char * PS_ARG_in_parameter_file,
                       char * PS_ARG_out_status_flag,
                       char * PS_ARG_out_status_messages );
/*
***************************************************************************************************
***************************************************************************************************
*/
StreetPerfectExportDeclaration
StreetPerfectCaptureAddress ( char * PS_ARG_in_parameter_file,
                              char * PS_CAN_in_postal_code,
                              char * PS_CAN_in_country,
                              char * PS_CAN_out_additional_information,
                              char * PS_CAN_out_delivery_information,
                              char * PS_CAN_out_extra_information,
                              char * PS_CAN_out_address_line,
                              char * PS_CAN_out_city,
                              char * PS_CAN_out_province,
                              char * PS_CAN_out_postal_code,
                              char * PS_CAN_out_country,
                              char * PS_CAN_out_format_line_one,
                              char * PS_CAN_out_format_line_two,
                              char * PS_CAN_out_format_line_three,
                              char * PS_ARG_out_status_flag,
                              char * PS_ARG_out_status_messages );
/*
***************************************************************************************************
***************************************************************************************************
*/
StreetPerfectExportDeclaration
StreetPerfectCorrectAddress ( char * PS_ARG_in_parameter_file,
                              char * PS_CAN_in_address_line,
                              char * PS_CAN_in_city,
                              char * PS_CAN_in_province,
                              char * PS_CAN_in_postal_code,
                              char * PS_CAN_in_country,
                              char * PS_CAN_out_address_line,
                              char * PS_CAN_out_city,
                              char * PS_CAN_out_province,
                              char * PS_CAN_out_postal_code,
                              char * PS_CAN_out_country,
                              char * PS_CAN_out_extra_information,
                              char * PS_CAN_out_unidentified_component,
                              char * PS_ARG_out_function_messages,
                              char * PS_ARG_out_status_flag,
                              char * PS_ARG_out_status_messages );
/*
***************************************************************************************************
***************************************************************************************************
*/
StreetPerfectExportDeclaration
StreetPerfectFetchAddress ( char * PS_ARG_in_parameter_file,
                            char * PS_CAN_in_street_number,
                            char * PS_CAN_in_unit_number,
                            char * PS_CAN_in_postal_code,
                            char * PS_CAN_out_address_line,
                            char * PS_CAN_out_city,
                            char * PS_CAN_out_province,
                            char * PS_CAN_out_postal_code,
                            char * PS_CAN_out_country,
                            char * PS_ARG_out_status_flag,
                            char * PS_ARG_out_status_messages );
/*
***************************************************************************************************
***************************************************************************************************
*/
StreetPerfectExportDeclaration
StreetPerfectFormatAddress ( char * PS_ARG_in_parameter_file,
                             char * PS_CAN_in_address_line,
                             char * PS_CAN_in_city,
                             char * PS_CAN_in_province,
                             char * PS_CAN_in_postal_code,
                             char * PS_CAN_in_country,
                             char * PS_CAN_out_format_line_one,
                             char * PS_CAN_out_format_line_two,
                             char * PS_CAN_out_format_line_three,
                             char * PS_CAN_out_format_line_four,
                             char * PS_CAN_out_format_line_five,
                             char * PS_ARG_out_status_flag,
                             char * PS_ARG_out_status_messages );
/*
***************************************************************************************************
***************************************************************************************************
*/
StreetPerfectExportDeclaration
StreetPerfectParseAddress ( char * PS_ARG_in_parameter_file,
                            char * PS_CAN_in_address_line,
                            char * PS_CAN_in_city,
                            char * PS_CAN_in_province,
                            char * PS_CAN_in_postal_code,
                            char * PS_CAN_in_country,
                            char * PS_CAN_xxx_address_type,
                            char * PS_CAN_out_address_line,
                            char * PS_CAN_out_street_number,
                            char * PS_CAN_out_street_suffix,
                            char * PS_CAN_out_street_name,
                            char * PS_CAN_out_street_type,
                            char * PS_CAN_out_street_direction,
                            char * PS_CAN_out_unit_type,
                            char * PS_CAN_out_unit_number,
                            char * PS_CAN_out_service_type,
                            char * PS_CAN_out_service_number,
                            char * PS_CAN_out_service_area_name,
                            char * PS_CAN_out_service_area_type,
                            char * PS_CAN_out_service_area_qualifier,
                            char * PS_CAN_out_city,
                            char * PS_CAN_out_city_abbrev_long,
                            char * PS_CAN_out_city_abbrev_short,
                            char * PS_CAN_out_province,
                            char * PS_CAN_out_postal_code,
                            char * PS_CAN_out_country,
                            char * PS_CAN_out_cpct_information,
                            char * PS_CAN_out_extra_information,
                            char * PS_CAN_out_unidentified_component,
                            char * PS_ARG_out_function_messages,
                            char * PS_ARG_out_status_flag,
                            char * PS_ARG_out_status_messages );
/*
***************************************************************************************************
***************************************************************************************************
*/
StreetPerfectExportDeclaration
StreetPerfectQueryAddress ( char * PS_ARG_in_parameter_file,
                            char * PS_CAN_in_query_option,
                            char * PS_CAN_in_address_line,
                            char * PS_CAN_in_city,
                            char * PS_CAN_in_province,
                            char * PS_CAN_in_postal_code,
                            char * PS_CAN_in_country,
                            char * PS_CAN_out_response_address_list,
                            char * PS_ARG_out_status_flag,
                            char * PS_ARG_out_status_messages );
/*
***************************************************************************************************
***************************************************************************************************
*/
StreetPerfectExportDeclaration
StreetPerfectSearchAddress ( char * PS_ARG_in_parameter_file,
                             char * PS_CAN_in_address_line,
                             char * PS_CAN_in_city,
                             char * PS_CAN_in_province,
                             char * PS_CAN_in_postal_code,
                             char * PS_CAN_in_country,
                             char * PS_CAN_out_response_address_list,
                             char * PS_ARG_out_status_flag,
                             char * PS_ARG_out_status_messages );
/*
***************************************************************************************************
***************************************************************************************************
*/
StreetPerfectExportDeclaration
StreetPerfectValidateAddress ( char * PS_ARG_in_parameter_file,
                               char * PS_CAN_in_address_line,
                               char * PS_CAN_in_city,
                               char * PS_CAN_in_province,
                               char * PS_CAN_in_postal_code,
                               char * PS_CAN_in_country,
                               char * PS_ARG_out_function_messages,
                               char * PS_ARG_out_status_flag,
                               char * PS_ARG_out_status_messages );
/*
***************************************************************************************************
***************************************************************************************************
*/
StreetPerfectExportDeclaration
StreetPerfectDisconnect ( char * PS_ARG_in_parameter_file,
                          char * PS_ARG_out_status_flag,
                          char * PS_ARG_out_status_messages );
/*
***************************************************************************************************
   American Address Definitions
***************************************************************************************************
*/
#define AmericanRecordTypeOffset                0
#define AmericanRecordTypeLength                3

#define AmericanZipCodeOffset                   3
#define AmericanZipCodeLength                   7

#define AmericanAddonLowOffset                  10
#define AmericanAddonLowLength                  6

#define AmericanAddonHighOffset                 16
#define AmericanAddonHighLength                 6

#define AmericanPrimaryLowOffset                22
#define AmericanPrimaryLowLength                12

#define AmericanPrimaryHighOffset               34
#define AmericanPrimaryHighLength               12

#define AmericanPreDirectionOffset              46
#define AmericanPreDirectionLength              4

#define AmericanStreetNameOffset                50
#define AmericanStreetNameLength                30

#define AmericanSuffixOffset                    80
#define AmericanSuffixLength                    6

#define AmericanPostDirectionOffset             86
#define AmericanPostDirectionLength             4

#define AmericanUnitOffset                      90
#define AmericanUnitLength                      6

#define AmericanSecondaryLowOffset              96
#define AmericanSecondaryLowLength              10

#define AmericanSecondaryHighOffset             106
#define AmericanSecondaryHighLength             10

#define AmericanFirmNameOffset                  116
#define AmericanFirmNameLength                  42

#define AmericanCityNameOffset                  158
#define AmericanCityNameLength                  30

#define AmericanStateAbbreviationOffset         188
#define AmericanStateAbbreviationLength         4

#define AmericanAddressBufferLength             192
/*
***************************************************************************************************
***************************************************************************************************
*/
typedef struct _AmericanAddressStructure
{
   char AmericanRecordType [ 2 ];
   char AmericanCityName [ 29 ];
   char AmericanStateAbbreviation [ 3 ];
   char AmericanZipCode [ 6 ];
   char AmericanAddonlLow [ 5 ];
   char AmericanAddonHigh [ 5 ];
   char AmericanPrimaryLow [ 11 ];
   char AmericanPrimaryHigh [ 11 ];
   char AmericanPreDirection [ 3 ];
   char AmericanStreetName [ 35 ];
   char AmericanSuffix [ 5 ];
   char AmericanPostDirection [ 3 ];
   char AmericanUnitType [ 5 ];
   char AmericanSecondaryLow [ 9 ];
   char AmericanSecondaryHigh [ 9 ];
   char AmericanFirmName [ 41 ];
} AmericanAddressStructure;

#define AmericanAddressStructureLength   sizeof ( struct _AmericanAddressStructure )

/*
***************************************************************************************************
   Canadian Address Definitions
***************************************************************************************************
*/
#define UrbanAddress                            "11"
#define UrbanRouteAddress                       "21"
#define RuralPoBoxAddress                       "32"
#define RuralRouteAddress                       "42"
#define RuralGdAddress                          "52"
/*
***************************************************************************************************
   The following describes the address buffer and structure
   used to define a CPC raw range data record.
   Either may be used to parse the address records
   returned after a call to the search function.
***************************************************************************************************
*/
/*
***************************************************************************************************
   address buffer fixed length field offsets and lengths
***************************************************************************************************
*/
#define AddressTypeCodeOffset                   0
#define AddressTypeCodeLength                   2

#define ProvinceCodeOffset                      2
#define ProvinceCodeLength                      2

#define DirectoryAreaNameOffset                 4
#define DirectoryAreaNameLength                 30

#define StreetNameOffset                        34
#define StreetNameLength                        30

#define StreetTypeCodeOffset                    64
#define StreetTypeCodeLength                    6

#define StreetDirectionCodeOffset               70
#define StreetDirectionCodeLength               2

#define StreetAddressSequenceCodeOffset         72
#define StreetAddressSequenceCodeLength         1

#define StreetNumberLastOffset                  73
#define StreetNumberLastLength                  6

#define StreetNumberLastSuffixCodeOffset        79
#define StreetNumberLastSuffixCodeLength        1

#define UnitNumberLastOffset                    80
#define UnitNumberLastLength                    6

#define StreetNumberFirstOffset                 86
#define StreetNumberFirstLength                 6

#define StreetNumberFirstSuffixCodeOffset       92
#define StreetNumberFirstSuffixCodeLength       1

#define UnitNumberFirstOffset                   93
#define UnitNumberFirstLength                   6

#define MunicipalityNameOffset                  99
#define MunicipalityNameLength                  30

#define RouteServiceBoxNumberLastOffset         129
#define RouteServiceBoxNumberLastLength         5

#define RouteServiceBoxNumberFirstOffset        134
#define RouteServiceBoxNumberFirstLength        5

#define RouteServiceTypeTwoDescriptorOffset     139
#define RouteServiceTypeTwoDescriptorLength     2

#define RouteServiceTypeTwoNumberOffset         141
#define RouteServiceTypeTwoNumberLength         4

#define DeliveryAreaNameOffset                  145
#define DeliveryAreaNameLength                  30

#define DeliveryAreaDescriptorOffset            175
#define DeliveryAreaDescriptorLength            5

#define DeliveryAreaQualifierOffset             180
#define DeliveryAreaQualifierLength             15

#define LockBoxBagNumberLastOffset              195
#define LockBoxBagNumberLastLength              5

#define LockBoxBagNumberFirstOffset             200
#define LockBoxBagNumberFirstLength             5

#define RouteServiceTypeFourDescriptorOffset    205
#define RouteServiceTypeFourDescriptorLength    2

#define RouteServiceTypeFourNumberOffset        207
#define RouteServiceTypeFourNumberLength        4

#define PostalCodeOffset                        211
#define PostalCodeLength                        10

#define DeliveryPostalCodeOffset                221
#define DeliveryPostalCodeLength                6

#define ActionCodeOffset                        227
#define ActionCodeLength                        1

#define TextRecordFlagOffset                    228
#define TextRecordFlagLength                    1

#define CountryCodeOffset                       229
#define CountryCodeLength                       3

#define AddressBufferLength                     232

/*
***************************************************************************************************

   address structure fixed length space padded fields

   memcpy the CPC raw range record to a variable of this type and retrieve
   the individual elements using a fixed length string copy as in:

   memcpy ( &'target_structure', 'source_cpc_string',
            min ( sizeof ( 'target_structure' ), strlen ( 'source_cpc_string' ) ) );
   memset ( 'target_string', '\0', sizeof ( 'target_string' ) );
   memcpy ( 'target_string', 'source_element',
            min ( sizeof ( 'target_string' ) - 1, sizeof ( 'source_element' ) ) );

   thus converting a fixed length address element to a null terminated address string

***************************************************************************************************
*/

typedef struct _CanadianAddressStructure
{
   char CanadianAddressTypeCode [ 2 ];                   /* "11"                             */
   char CanadianProvinceCode [ 2 ];                      /* "ON"                             */
   char CanadianDirectoryAreaName [ 30 ];                /* "TORONTO                       " */
   char CanadianStreetName [ 30 ];                       /* "EMPIRE                        " */
   char CanadianStreetTypeCode [ 6 ];                    /* "AVE   "                         */
   char CanadianStreetDirectionCode [ 2 ];               /* "  "                             */
   char CanadianStreetAddressSequenceCode [ 1 ];         /* "2"                              */
   char CanadianStreetNumberLast [ 6 ];                  /* "000094"                         */
   char CanadianStreetNumberLastSuffixCode [ 1 ];        /* " "                              */
   char CanadianUnitNumberLast [ 6 ];                    /* "      "                         */
   char CanadianStreetNumberFirst [ 6 ];                 /* "000002"                         */
   char CanadianStreetNumberFirstSuffixCode [ 1 ];       /* " "                              */
   char CanadianUnitNumberFirst [ 6 ];                   /* "      "                         */
   char CanadianMunicipalityName [ 30 ];                 /* "TORONTO                       " */
   char CanadianRouteServiceBoxNumberLast [ 5 ];         /* "     "                          */
   char CanadianRouteServiceBoxNumberFirst [ 5 ];        /* "     "                          */
   char CanadianRouteServiceTypeTwoDescriptor [ 2 ];     /* "  "                             */
   char CanadianRouteServiceTypeTwoNumber [ 4 ];         /* "    "                           */
   char CanadianDeliveryAreaName [ 30 ];                 /* "                              " */
   char CanadianDeliveryAreaDescriptor [ 5 ];            /* "     "                          */
   char CanadianDeliveryAreaQualifier [ 15 ];            /* "               "                */
   char CanadianLockBoxBagNumberLast [ 5 ];              /* "     "                          */
   char CanadianLockBoxBagNumberFirst [ 5 ];             /* "     "                          */
   char CanadianRouteServiceTypeFourDescriptor [ 2 ];    /* "  "                             */
   char CanadianRouteServiceTypeFourNumber [ 4 ];        /* "    "                           */
   char CanadianPostalCode [ 10 ];                       /* "M4M2L4    "                     */
   char CanadianDeliveryPostalCode [ 6 ];                /* "      "                         */
   char CanadianActionCode [ 1 ];                        /* " "                              */
   char CanadianTextRecordFlag [ 1 ];                    /* "N"                              */
   char CanadianCountryCode [ 3 ];                       /* "   "                            */
}  CanadianAddressStructure;

#define CanadianAddressStructureLength   sizeof ( struct _CanadianAddressStructure )

/*
***************************************************************************************************
   Additional buffer sizes dependent on the specific API fuction
   NOTE: All buffers must be space padded and null terminated.
***************************************************************************************************
*/

#define TextStatusLength            1
#define TextMessageLength           132

#define QueryMinimumLength          10

#define Query11RecordType           11
#define Query11RecordText           "Formatted range records and CPC raw range records for Postal Code"
#define Query11RecordLength         AddressBufferLength

#define Query12RecordType           12
#define Query12RecordText           "Additional text information for Postal Code"
#define Query12RecordLength         TextMessageLength

#define Query14RecordType           14
#define Query14RecordText           "Paired 63-byte internal format address records for CPC range record"
#define Query14RecordLength         AddressBufferLength

#define Query16RecordType           16
#define Query16RecordText           "Postal Codes's for city and province"
#define Query16RecordLength         PostalCodeLength

#define Query20RecordType           20
#define Query20RecordText           "Rural Search ( city, or city & province )"
#define Query20RecordLength         AddressBufferLength

#define Query21RecordType           21
#define Query21RecordText           "Urban Search ( street name, city, or city & province )"
#define Query21RecordLength         AddressBufferLength

#define Query22RecordType           22
#define Query22RecordText           "Urban Route Search  ( street name, city, or city & province )"
#define Query22RecordLength         AddressBufferLength

#define Query23RecordType           23
#define Query23RecordText           "Rural 'PO BOX' Search ( city, or city & province )"
#define Query23RecordLength         AddressBufferLength

#define Query24RecordType           24
#define Query24RecordText           "Rural 'RR/SS/MR' Search Types ( city, or city & province )"
#define Query24RecordLength         AddressBufferLength

#define Query25RecordType           25
#define Query25RecordText           "Rural 'GD' Search ( city, or city & province )"
#define Query25RecordLength         AddressBufferLength

#define Query26RecordType           26
#define Query26RecordText           "CPC raw range record search ( CPC raw range record )"
#define Query26RecordLength         AddressBufferLength

#define Query31RecordType           31
#define Query31RecordText           "Street type table entries"
#define Query31RecordLength         28

#define Query32RecordType           32
#define Query32RecordText           "Street direction table entries"
#define Query32RecordLength         QueryMinimumLength /* 2 */

#define Query33RecordType           33
#define Query33RecordText           "Route service type description table entries"
#define Query33RecordLength         28

#define Query34RecordType           34
#define Query34RecordText           "Province code table entries"
#define Query34RecordLength         56

#define Query35RecordType           35
#define Query35RecordText           "Service type table entries"
#define Query35RecordLength         QueryMinimumLength /* 6 */

#define Query36RecordType           36
#define Query36RecordText           "Delivery installation type table entries"
#define Query36RecordLength         37

#define Query37RecordType           37
#define Query37RecordText           "Country code table entries"
#define Query37RecordLength         35

#define Query38RecordType           38
#define Query38RecordText           "US state code table entries"
#define Query38RecordLength         44

#define Query39RecordType           39
#define Query39RecordText           "Unit designator table entries"
#define Query39RecordLength         25

#define Query310RecordType          310
#define Query310RecordText          "Street name table entries ( partial or full street name )"
#define Query310RecordLength        StreetNameLength

#define Query311RecordType          311
#define Query311RecordText          "Urban municipality table entries ( partial or full city name )"
#define Query311RecordLength        MunicipalityNameLength

#define Query312RecordType          312
#define Query312RecordText          "Rural municipality table entries ( partial or full city name )"
#define Query312RecordLength        MunicipalityNameLength

#define Query313RecordType          313
#define Query313RecordText          "All municipality table entries ( partial or full city name )"
#define Query313RecordLength        MunicipalityNameLength

#define Query314RecordType          314
#define Query314RecordText          "Municipality Abbreviations given Municipality and Province"
#define Query314RecordLength        2*MunicipalityNameLength

#define Query315RecordType          315
#define Query315RecordText          "CPC Urban Extra Information"
#define Query315RecordLength        24

#define Query316RecordType          316
#define Query316RecordText          "CPC Rural Extra Information"
#define Query316RecordLength        24

#define Query42RecordType           42
#define Query42RecordText           "Postal Codes For CPC Text Information - Requires Address Line"
#define Query42RecordLength         74

#define Query43RecordType           43
#define Query43RecordText           "Municipalities Within Province - Requires Province Code"
#define Query43RecordLength         32

#define Query44RecordType           44
#define Query44RecordText           "Streets Within Municipality - Requires Municipality And Province Code"
#define Query44RecordLength         32

#define Query99RecordType           99
#define Query99RecordText           "System Information"
#define Query99RecordLength         100

/*
***************************************************************************************************
***************************************************************************************************
*/

#ifdef __cplusplus
}
#endif

/*
***************************************************************************************************
***************************************************************************************************
*/

