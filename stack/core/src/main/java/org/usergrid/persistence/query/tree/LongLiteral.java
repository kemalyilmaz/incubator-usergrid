/*******************************************************************************
 * Copyright 2012 Apigee Corporation
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 ******************************************************************************/
package org.usergrid.persistence.query.tree;


import org.antlr.runtime.ClassicToken;
import org.antlr.runtime.Token;


/** @author tnine */
public class LongLiteral extends Literal<Long> implements NumericLiteral {

    private long value;


    /**
     * @param t
     */
    public LongLiteral( Token t ) {
        super( t );
        this.value = Long.valueOf( t.getText() );
    }


    /**
     *
     * @param value
     */
    public LongLiteral( long value ) {
        super( new ClassicToken( 0, String.valueOf( value ) ) );
        this.value = value;
    }


    /**
     *
     * @return
     */
    public Long getValue() {
        return this.value;
    }


    /* (non-Javadoc)
     * @see org.usergrid.persistence.query.tree.NumericLiteral#getFloatValue()
     */
    @Override
    public float getFloatValue() {
        return value;
    }
}
